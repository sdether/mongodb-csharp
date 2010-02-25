using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using MongoDB.Driver;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Mapping.CollectionTypes;
using MongoDB.Framework.Mapping.ValueConverters;
using MongoDB.Framework.Mapping.IdGenerators;
using MongoDB.Framework.Reflection;
using MongoDB.Framework.Configuration.Fluent.Mapping;
using MongoDB.Framework.Configuration.Mapping.Visitors;

namespace MongoDB.Framework.Configuration.Mapping
{
    public class MapModelRegistry : IMapModelRegistry
    {
        #region Private Fields

        private Dictionary<Type, RootClassMapModel> rootClassMapModels;
        private Dictionary<Type, NestedClassMapModel> nestedClassMapModels;
        private Dictionary<Type, SubClassMapModel> subClassMapModels;

        private Dictionary<Type, RootClassMap> rootClassMaps;
        private Dictionary<Type, NestedClassMap> nestedClassMaps;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelledMapProvider"/> class.
        /// </summary>
        public MapModelRegistry()
        {
            this.rootClassMapModels = new Dictionary<Type, RootClassMapModel>();
            this.nestedClassMapModels = new Dictionary<Type, NestedClassMapModel>();
            this.subClassMapModels = new Dictionary<Type, SubClassMapModel>();
        }

        #endregion

        #region Public Methods

        public void AddModel(RootClassMapModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            this.rootClassMapModels.Add(model.Type, model);
        }

        /// <summary>
        /// Adds the nested class map model.
        /// </summary>
        /// <param name="nestedClassMapModel">The nested class map model.</param>
        public void AddModel(NestedClassMapModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            this.nestedClassMapModels.Add(model.Type, model);
        }

        /// <summary>
        /// Adds the sub class map model.
        /// </summary>
        /// <param name="subClassMapModel">The sub class map model.</param>
        public void AddModel(SubClassMapModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            this.subClassMapModels.Add(model.Type, model);
        }

        /// <summary>
        /// Adds the source.
        /// </summary>
        /// <param name="classMapModelSource">The class map model source.</param>
        public void AddSource(IClassMapModelSource classMapModelSource)
        {
            foreach (var model in classMapModelSource.GetClassMapModels())
            {
                if (model is RootClassMapModel)
                    this.AddModel((RootClassMapModel)model);
                else if (model is NestedClassMapModel)
                    this.AddModel((NestedClassMapModel)model);
                else if (model is SubClassMapModel)
                    this.AddModel((SubClassMapModel)model);
                else
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Builds the mapping store.
        /// </summary>
        /// <returns></returns>
        public IMappingStore BuildMappingStore()
        {
            this.BuildRootClassMaps();
            return new MappingStore(this.rootClassMaps.Values);
        }

        #endregion

        #region Private Methods

        private void AssociateFreeSubClassMapsWithSupers()
        {
            Func<Type, Func<Type, bool>, Type> getSuperClassType = null;
            getSuperClassType = (type, containsKey) =>
            {
                var baseType = type.BaseType;
                if(baseType == typeof(object))
                    return null;

                if(containsKey(baseType))
                    return baseType;

                return getSuperClassType(baseType, containsKey);
            };

            var subs = new List<SubClassMapModel>(this.subClassMapModels.Values);
            foreach (var sub in subs)
            {
                var superClassType = getSuperClassType(sub.Type, this.rootClassMapModels.ContainsKey);
                if (superClassType != null)
                {
                    this.rootClassMapModels[superClassType].SubClassMaps.Add(sub);
                    this.subClassMapModels.Remove(sub.Type);
                    continue;
                }
                superClassType = getSuperClassType(sub.Type, this.nestedClassMapModels.ContainsKey);
                if (superClassType != null)
                {
                    this.nestedClassMapModels[superClassType].SubClassMaps.Add(sub);
                    this.subClassMapModels.Remove(sub.Type);
                }
            }
        }

        private void ApplyConventions()
        {
            var runner = new ConventionsRunner(this.rootClassMapModels.Keys);
            foreach (var rootClassMapModel in this.rootClassMapModels.Values.Where(m => m.Conventions != null))
                runner.ApplyConventions(rootClassMapModel);

            foreach (var nestedClassMapModel in this.nestedClassMapModels.Values.Where(m => m.Conventions != null))
                runner.ApplyConventions(nestedClassMapModel);

            foreach (var subClassMapModel in this.subClassMapModels.Values.Where(m => m.Conventions != null))
                runner.ApplyConventions(subClassMapModel);
        }

        private void BuildRootClassMaps()
        {
            this.AssociateFreeSubClassMapsWithSupers();
            this.ApplyConventions();
            this.rootClassMaps = new Dictionary<Type, RootClassMap>();
            this.nestedClassMaps = new Dictionary<Type, NestedClassMap>();

            foreach (var rootClassMapModel in this.rootClassMapModels.Values)
                this.BuildRootClassMap(rootClassMapModel);
        }

        private void BuildRootClassMap(RootClassMapModel model)
        {
            new DuplicateMembersFinder()
                .FindAndClean(model);

            var rootClassMap = new RootClassMap(model.Type)
            {
                CollectionName = model.CollectionName,
                IdMap = this.BuildIdMap(model.IdMap),
                Discriminator = model.Discriminator,
                DiscriminatorKey = model.DiscriminatorKey,
                ExtendedPropertiesMap = this.BuildExtendedPropertiesMap(model.ExtendedPropertiesMap)
            };

            if (model.ClassActivatorType == null)
                rootClassMap.ClassActivator = new DefaultClassActivator();
            else
                rootClassMap.ClassActivator = (IClassActivator)Activator.CreateInstance(model.ClassActivatorType);

            this.rootClassMaps.Add(rootClassMap.Type, rootClassMap);

            var memberMaps = model.PersistentMemberMaps.Select(v => this.BuildMemberMap(v));
            var subClassMaps = model.SubClassMaps.Select(sc => this.BuildSubClassMap(sc));
            var indices = model.Indexes.Select(i => this.BuildIndex(i));

            rootClassMap.AddMemberMaps(memberMaps);
            rootClassMap.AddSubClassMaps(subClassMaps);
            rootClassMap.AddIndices(indices);
        }

        private void BuildNestedClassMap(NestedClassMapModel model)
        {
            new DuplicateMembersFinder()
                .FindAndClean(model);

            var extPropMap = this.BuildExtendedPropertiesMap(model.ExtendedPropertiesMap);
            IdMap idMap = null;
            if(model.IdMap != null)
                idMap = this.BuildIdMap(model.IdMap);

            var nestedClassMap = new NestedClassMap(model.Type)
            {
                IdMap = idMap,
                Discriminator = model.Discriminator,
                DiscriminatorKey = model.DiscriminatorKey,
                ExtendedPropertiesMap = extPropMap
            };
            this.nestedClassMaps.Add(model.Type, nestedClassMap);

            var memberMaps = model.PersistentMemberMaps.Select(v => this.BuildMemberMap(v)).ToList();
            if (model.ParentMap != null)
                memberMaps.Add(this.BuildParentMemberMap(model.ParentMap));

            var subClassMaps = model.SubClassMaps.Select(sc => this.BuildSubClassMap(sc));

            nestedClassMap.AddMemberMaps(memberMaps);
            nestedClassMap.AddSubClassMaps(subClassMaps);
        }

        private SubClassMap BuildSubClassMap(SubClassMapModel model)
        {
            var memberMaps = model.PersistentMemberMaps.Select(v => this.BuildMemberMap(v));

            var subClassMap = new SubClassMap(model.Type)
            {
                Discriminator = model.Discriminator
            };

            subClassMap.AddMemberMaps(memberMaps);

            return subClassMap;
        }

        private ExtendedPropertiesMap BuildExtendedPropertiesMap(ExtendedPropertiesMapModel model)
        {
            if (model == null)
                return null;

            var getter = LateBoundReflection.GetGetter(model.Getter);
            var setter = LateBoundReflection.GetSetter(model.Setter);
            return new ExtendedPropertiesMap(model.Getter.Name,
                getter,
                setter);
        }

        private IdMap BuildIdMap(IdMapModel model)
        {
            var getter = LateBoundReflection.GetGetter(model.Getter);
            var setter = LateBoundReflection.GetSetter(model.Setter);
            IIdGenerator generator = model.Generator;
            object unsavedValue = model.UnsavedValue;

            var memberType = ReflectionUtil.GetMemberValueType(model.Getter);
            //TODO: make this a convention
            if (memberType == typeof(Oid) && generator == null)
            {
                generator = new MongoDB.Framework.Mapping.IdGenerators.OidGenerator();
            }
            else if (memberType == typeof(Guid) && generator == null)
                generator = new GuidCombGenerator();

            //TODO: make this a convention
            if (unsavedValue == null)
                unsavedValue = memberType.IsValueType ? Activator.CreateInstance(memberType) : null;

            return new IdMap(model.Getter.Name, getter, setter, generator, model.ValueConverter, unsavedValue);
        }

        private Index BuildIndex(IndexModel model)
        {
            if (model.Name == null)
                model.Name = model.Parts.Aggregate("ix", (c, p) => c + "_" + p.Key);

            return new Index(model.Name, model.Parts, model.IsUnique);
        }

        private MemberMap BuildMemberMap(PersistentMemberMapModel model)
        {
            var getter = LateBoundReflection.GetGetter(model.Getter);
            var setter = LateBoundReflection.GetSetter(model.Setter);
            string name = model.Getter.Name;
            string key = model.Key ?? name;
            var memberValueType = ReflectionUtil.GetMemberValueType(model.Getter);
            var persistNull = model.PersistNull;

            ValueTypeBase valueType = null;

            if (model is ConvertibleMemberMapModel)
            {
                valueType = this.GetValueType(((ConvertibleMemberMapModel)model).ValueConverter, memberValueType);
            }
            else if (model is CollectionMemberMapModel)
            {
                valueType = this.GetCollectionValueType((CollectionMemberMapModel)model);
            }
            else if (model is ManyToOneMapModel)
            {
                bool isLazy = ((ManyToOneMapModel)model).IsLazy.Value;
                valueType = new ManyToOneValueType(memberValueType, isLazy);
            }
            else
                throw new NotSupportedException("Unknown PersistentMemberMapModel.");

            return new ValueTypeMemberMap(
                key,
                name,
                getter,
                setter,
                persistNull,
                valueType);
        }

        private ParentMemberMap BuildParentMemberMap(ParentMemberMapModel model)
        {
            var getter = LateBoundReflection.GetGetter(model.Getter);
            var setter = LateBoundReflection.GetSetter(model.Setter);
            string name = model.Getter.Name;

            return new ParentMemberMap(name, getter, setter);
        }

        private NestedClassMap TryGetNestedClassMapFor(Type type)
        {
            NestedClassMap nestedClassMap;
            if (this.nestedClassMaps.TryGetValue(type, out nestedClassMap))
                return nestedClassMap;

            NestedClassMapModel nestedClassMapModel;
            if (!this.nestedClassMapModels.TryGetValue(type, out nestedClassMapModel))
                return null;

            this.BuildNestedClassMap(nestedClassMapModel);
            return this.nestedClassMaps[type];
        }

        private ValueTypeBase GetValueType(IValueConverter valueConverter, Type type)
        {
            NestedClassMap nestedClassMap = this.TryGetNestedClassMapFor(type);
            if(nestedClassMap == null)
                return new SimpleValueType(type, valueConverter);

            return new NestedClassValueType(nestedClassMap, valueConverter);
        }

        private CollectionValueType GetCollectionValueType(CollectionMemberMapModel model)
        {
            var elementType = this.GetValueType(new NullSafeValueConverter(model.ElementType), model.ElementType);
            return new CollectionValueType(model.CollectionType, elementType);
        }

        #endregion
    }
}