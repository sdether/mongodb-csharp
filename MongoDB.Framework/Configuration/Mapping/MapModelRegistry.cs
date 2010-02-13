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

namespace MongoDB.Framework.Configuration.Mapping
{
    public class MapModelRegistry : IMapModelRegistry
    {
        #region Private Fields

        private Dictionary<Type, Func<Type, Type>> elementTypeFactories = new Dictionary<Type, Func<Type, Type>>()        {            { typeof(ICollection<>), mt => mt.GetGenericArguments()[0] },            { typeof(IList<>), mt => mt.GetGenericArguments()[0] },            { typeof(List<>), mt => mt.GetGenericArguments()[0] },            { typeof(HashSet<>), mt => mt.GetGenericArguments()[0] },            { typeof(IDictionary<,>), mt => mt.GetGenericArguments()[1] },            { typeof(Dictionary<,>), mt => mt.GetGenericArguments()[1] },        };

        private Dictionary<Type, RootClassMapModel> rootClassMapModels;
        private Dictionary<Type, NestedClassMapModel> nestedClassMapModels;
        private Dictionary<Type, SubClassMapModel> subClassMapModels;

        private Dictionary<Type, RootClassMap> rootClassMaps;
        private Dictionary<Type, NestedClassMap> nestedClassMaps;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the root class map models.
        /// </summary>
        /// <value>The root class map models.</value>
        public IEnumerable<RootClassMapModel> RootClassMapModels
        {
            get { return this.rootClassMapModels.Values; }
        }

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

        /// <summary>
        /// Adds the root class map model.
        /// </summary>
        /// <param name="rootClassMapModel">The root class map model.</param>
        public void AddRootClassMapModel(RootClassMapModel rootClassMapModel)
        {
            if (rootClassMapModel == null)
                throw new ArgumentNullException("rootClassMapModel");

            this.rootClassMapModels.Add(rootClassMapModel.Type, rootClassMapModel);
        }

        /// <summary>
        /// Adds the nested class map model.
        /// </summary>
        /// <param name="nestedClassMapModel">The nested class map model.</param>
        public void AddNestedClassMapModel(NestedClassMapModel nestedClassMapModel)
        {
            if (nestedClassMapModel == null)
                throw new ArgumentNullException("nestedClassMapModel");

            this.nestedClassMapModels.Add(nestedClassMapModel.Type, nestedClassMapModel);
        }

        /// <summary>
        /// Adds the sub class map model.
        /// </summary>
        /// <param name="subClassMapModel">The sub class map model.</param>
        public void AddSubClassMapModel(SubClassMapModel subClassMapModel)
        {
            if (subClassMapModel == null)
                throw new ArgumentNullException("subClassMapModel");

            this.subClassMapModels.Add(subClassMapModel.Type, subClassMapModel);
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
            foreach(var sub in subs)
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
                    this.nestedClassMapModels.Add(sub.Type, nestedClassMapModels[superClassType]);
                    this.subClassMapModels.Remove(sub.Type);
                }
            }
        }

        private void BuildRootClassMaps()
        {
            this.AssociateFreeSubClassMapsWithSupers();
            this.rootClassMaps = new Dictionary<Type, RootClassMap>();
            this.nestedClassMaps = new Dictionary<Type, NestedClassMap>();

            foreach (var rootClassMapModel in this.rootClassMapModels.Values)
                this.BuildRootClassMap(rootClassMapModel);
        }

        private void BuildRootClassMap(RootClassMapModel model)
        {
            var extPropMap = this.BuildExtendedPropertiesMap(model.ExtendedPropertiesMap);
            var idMap = this.BuildIdMap(model.IdMap);
            string collectionName = model.CollectionName ?? model.Type.Name;

            var rootClassMap = new RootClassMap(model.Type)
            {
                CollectionName = collectionName,
                IdMap = idMap,
                Discriminator = model.Discriminator,
                DiscriminatorKey = model.DiscriminatorKey,
                ExtendedPropertiesMap = extPropMap,
            };

            this.rootClassMaps.Add(model.Type, rootClassMap);

            var memberMaps = model.ValueMaps.Select(v => this.BuildMemberMap(v))
                .Concat(model.CollectionMaps.Select(c => this.BuildMemberMap(c)))
                .Concat(model.ManyToOneMaps.Select(mto => this.BuildMemberMap(mto)))
                .ToList();
            var subClassMaps = model.SubClassMaps.Select(sc => this.BuildSubClassMap(sc));
            var indices = model.Indexes.Select(i => this.BuildIndex(i));

            rootClassMap.AddMemberMaps(memberMaps);
            rootClassMap.AddSubClassMaps(subClassMaps);
            rootClassMap.AddIndices(indices);
        }

        private void BuildNestedClassMap(NestedClassMapModel model)
        {
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

            var memberMaps = model.ValueMaps.Select(v => this.BuildMemberMap(v))
                .Concat(model.CollectionMaps.Select(c => this.BuildMemberMap(c)))
                .Concat(model.ManyToOneMaps.Select(mto => this.BuildMemberMap(mto)))
                .ToList();
            if (model.ParentMap != null)
                memberMaps.Add(this.BuildParentMemberMap(model.ParentMap));

            var subClassMaps = model.SubClassMaps.Select(sc => this.BuildSubClassMap(sc));

            nestedClassMap.AddMemberMaps(memberMaps);
            nestedClassMap.AddSubClassMaps(subClassMaps);
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
            IValueConverter valueConverter = model.ValueConverter;

            var memberType = ReflectionUtil.GetMemberValueType(model.Getter);
            if (memberType == typeof(Oid) && generator == null)
            {
                generator = new MongoDB.Framework.Mapping.IdGenerators.OidGenerator();
            }
            else if (memberType == typeof(Guid) && generator == null)
                generator = new GuidCombGenerator();

            if (valueConverter == null)
                valueConverter = this.GetValueConverterForType(memberType);

            if (unsavedValue == null)
                unsavedValue = memberType.IsValueType ? Activator.CreateInstance(memberType) : null;

            return new IdMap(model.Getter.Name, getter, setter, generator, valueConverter, unsavedValue);
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
                var memberMapModel = (ConvertibleMemberMapModel)model;
                if (memberMapModel.ValueConverter != null)
                    valueType = new SimpleValueType(memberValueType, memberMapModel.ValueConverter ?? this.GetValueConverterForType(memberValueType));
                else
                    valueType = this.GetValueTypeForType(memberValueType);
            }
            else if (model is CollectionMemberMapModel)
            {
                valueType = this.GetCollectionValueType(memberValueType, (CollectionMemberMapModel)model);
            }
            else if (model is ManyToOneMapModel)
            {
                bool isLazy = ((ManyToOneMapModel)model).IsLazy;

                if (memberValueType.IsSealed)
                    isLazy = false;

                valueType = new ManyToOneValueType(memberValueType, isLazy);
            }
            else
                throw new NotSupportedException("Unknown MemberMapModelBase.");

            return new ValueTypeMemberMap(
                key,
                name,
                getter,
                setter,
                persistNull,
                valueType);
        }

        private SubClassMap BuildSubClassMap(SubClassMapModel model)
        {
            var memberMaps = model.ValueMaps.Select(v => this.BuildMemberMap(v))
                .Concat(model.CollectionMaps.Select(c => this.BuildMemberMap(c)))
                .Concat(model.ManyToOneMaps.Select(mto => this.BuildMemberMap(mto)))
                .ToList(); 
            
            var subClassMap = new SubClassMap(model.Type)
            {
                Discriminator = model.Discriminator
            };

            subClassMap.AddMemberMaps(memberMaps);

            return subClassMap;
        }

        private ParentMemberMap BuildParentMemberMap(ParentMemberMapModel model)
        {
            var getter = LateBoundReflection.GetGetter(model.Getter);
            var setter = LateBoundReflection.GetSetter(model.Setter);
            string name = model.Getter.Name;

            return new ParentMemberMap(name, getter, setter);
        }

        private bool IsCollection(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string);
        }

        private NestedClassMap GetNestedClassMapFor(Type type)
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

        private ValueTypeBase GetValueTypeForType(Type type)
        {
            NestedClassMap nestedClassMap = this.GetNestedClassMapFor(type);
            if(nestedClassMap == null)
                return new SimpleValueType(type, this.GetValueConverterForType(type));

            return new NestedClassValueType(nestedClassMap);
        }

        private IValueConverter GetValueConverterForType(Type type)
        {
            if (type == typeof(Guid))
                return new GuidValueConverter();

            if (type == typeof(Regex))
            {
                //TODO: create a regex value type
            }

            return new NullSafeValueConverter(type);
        }

        private CollectionValueType GetCollectionValueType(Type memberType, CollectionMemberMapModel model)
        {
            if (model.CollectionType == null)
                model.CollectionType = this.GetCollectionType(memberType);

            if (model.ElementValueType == null)
            {
                if (model.ElementType == null)
                    model.ElementType = this.DiscoverCollectionElementType(memberType);

                model.ElementValueType = this.GetValueTypeForType(model.ElementType);
            }

            return new CollectionValueType(model.CollectionType, model.ElementValueType);
        }

        private Type DiscoverCollectionElementType(Type memberType)
        {
            if (memberType.IsGenericType)
            {
                var genType = memberType.GetGenericTypeDefinition();
                if (genType == typeof(IList<>) || genType == typeof(List<>) || genType == typeof(ICollection<>) || genType == typeof(HashSet<>))
                    return memberType.GetGenericArguments()[0];
                if (genType == typeof(IDictionary<,>) || genType == typeof(Dictionary<,>) && memberType.GetGenericArguments()[0] == typeof(string))
                    return memberType.GetGenericArguments()[1];
            }

            throw new NotSupportedException(string.Format("Could not discover element type from {0}.", memberType));
        }

        private ICollectionType GetCollectionType(Type memberType)
        {
            if (memberType.IsGenericType)
            {
                var genType = memberType.GetGenericTypeDefinition();
                if (genType == typeof(IList<>) || genType == typeof(List<>) || genType == typeof(ICollection<>))
                    return new GenericListCollectionType();
                if (genType == typeof(HashSet<>))
                    return new HashSetCollectionType();
                if (genType == typeof(IDictionary<,>) || genType == typeof(Dictionary<,>) && memberType.GetGenericArguments()[0] == typeof(string))
                    return new GenericStringDictionaryCollectionType();
            }

            throw new NotSupportedException(string.Format("Could not create collection type from {0}.", memberType));
        }


        #endregion
    }
}