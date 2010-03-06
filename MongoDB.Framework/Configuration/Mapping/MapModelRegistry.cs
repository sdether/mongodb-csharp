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

namespace MongoDB.Framework.Configuration.Mapping
{
    public class MapModelRegistry : IMapModelRegistry
    {
        #region Private Fields

        private Dictionary<Type, ClassMapModel> classMapModels;
        private Dictionary<Type, SubClassMapModel> subClassMapModels;

        private Dictionary<Type, ClassMap> classMaps;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelledMapProvider"/> class.
        /// </summary>
        public MapModelRegistry()
        {
            this.classMapModels = new Dictionary<Type, ClassMapModel>();
            this.subClassMapModels = new Dictionary<Type, SubClassMapModel>();
        }

        #endregion

        #region Public Methods

        public void AddModel(ClassMapModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            this.classMapModels.Add(model.Type, model);
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
        /// Builds the mapping store.
        /// </summary>
        /// <returns></returns>
        public IMappingStore BuildMappingStore()
        {
            this.AssociateFreeSubClassMapsWithSupers();
            this.BuildClassMaps();
            return new MappingStore(this.classMaps.Values);
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
                var superClassType = getSuperClassType(sub.Type, this.classMapModels.ContainsKey);
                if (superClassType != null)
                {
                    this.classMapModels[superClassType].SubClassMaps.Add(sub);
                    this.subClassMapModels.Remove(sub.Type);
                    continue;
                }
            }
        }

        private void BuildClassMaps()
        {
            this.classMaps = new Dictionary<Type, ClassMap>();

            foreach (var classMapModel in this.classMapModels.Values)
                this.BuildClassMap(classMapModel);
        }

        private void BuildClassMap(ClassMapModel model)
        {
            var classMap = new ClassMap(model.Type)
            {
                ClassActivator = model.ClassActivator ?? DefaultClassActivator.Instance,
                CollectionName = model.CollectionName ?? model.Type.Name,
                IdMap = this.BuildIdMap(model.IdMap),
                Discriminator = model.Discriminator,
                DiscriminatorKey = model.DiscriminatorKey,
                ExtendedPropertiesMap = this.BuildExtendedPropertiesMap(model.ExtendedPropertiesMap)
            };

            this.classMaps.Add(classMap.Type, classMap);

            var memberMaps = model.PersistentMemberMaps.Select(v => this.BuildMemberMap(v)).ToList();
            if (model.ParentMemberMap != null)
                memberMaps.Add(this.BuildParentMemberMap(model.ParentMemberMap));

            var subClassMaps = model.SubClassMaps.Select(sc => this.BuildSubClassMap(sc));
            var indices = model.Indexes.Select(i => this.BuildIndex(i));

            classMap.AddMemberMaps(memberMaps);
            classMap.AddSubClassMaps(subClassMaps);
            classMap.AddIndices(indices);
        }

        private SubClassMap BuildSubClassMap(SubClassMapModel model)
        {
            var subClassMap = new SubClassMap(model.Type)
            {
                ClassActivator = DefaultClassActivator.Instance,
                Discriminator = model.Discriminator
            };

            var memberMaps = model.PersistentMemberMaps.Select(v => this.BuildMemberMap(v)).ToList();
            if (model.ParentMemberMap != null)
                memberMaps.Add(this.BuildParentMemberMap(model.ParentMemberMap));

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
            if (model == null)
                return null;

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

            return new IdMap(model.Getter.Name, getter, setter, generator, model.ValueConverter ?? this.GetValueConverter(memberType), unsavedValue);
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
            string memberName = model.Getter.Name;
            string key = model.Key ?? memberName;
            var memberValueType = ReflectionUtil.GetMemberValueType(model.Getter);
            var persistNull = model.PersistNull;
            ValueTypeBase valueType = null;

            //this model needs to be up-converted to a mappable model...
            if (model.GetType() == typeof(PersistentMemberMapModel))
            {
                if (this.IsCollection(memberValueType))
                {
                    model = new CollectionMemberMapModel()
                    {
                        Key = model.Key,
                        Getter = model.Getter,
                        Setter = model.Setter,
                        PersistNull = model.PersistNull,
                        CollectionType = this.GetCollectionType(memberValueType),
                        ElementType = this.GetElementType(memberValueType)
                    };
                }
                else
                {
                    model = new ConvertibleMemberMapModel()
                    {
                        Key = model.Key,
                        Getter = model.Getter,
                        Setter = model.Setter,
                        PersistNull = model.PersistNull,
                    };
                }
            }

            if (model is ConvertibleMemberMapModel)
            {
                valueType = this.GetValueType(((ConvertibleMemberMapModel)model).ValueConverter, memberValueType);
            }
            else if (model is CollectionMemberMapModel)
            {
                valueType = this.GetCollectionValueType((CollectionMemberMapModel)model);
            }
            else if (model is ReferenceMapModel)
            {
                bool isLazy = ((ReferenceMapModel)model).IsLazy;
                valueType = new ManyToOneValueType(memberValueType, isLazy);
            }
            else
                throw new NotSupportedException("Unknown type of PersistentMemberMapModel.");

            return new ValueTypeMemberMap(
                key,
                memberName,
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

        private ValueTypeBase GetValueType(IValueConverter valueConverter, Type type)
        {
            if (valueConverter == null)
                valueConverter = this.GetValueConverter(type);
            
            if(this.classMaps.ContainsKey(type) || this.classMapModels.ContainsKey(type)) //TODO: deal with references directly to subclasses.
                return new NestedClassValueType(type, valueConverter);

            return new SimpleValueType(type, valueConverter);
        }

        private CollectionValueType GetCollectionValueType(CollectionMemberMapModel model)
        {
            var type = ReflectionUtil.GetMemberValueType(model.Getter);
            if (model.CollectionType == null)
                model.CollectionType = this.GetCollectionType(type);
            if (model.ElementType == null)
                model.ElementType = this.GetElementType(type);
            if(model.ElementValueType == null)
                model.ElementValueType = this.GetValueType(new NullSafeValueConverter(model.ElementType), model.ElementType);
            return new CollectionValueType(model.CollectionType, model.ElementValueType);
        }

        private ICollectionType GetCollectionType(Type type)
        {
            if (type.IsGenericType)
            {
                var genType = type.GetGenericTypeDefinition();
                if (genType == typeof(IList<>) || genType == typeof(List<>) || genType == typeof(ICollection<>))
                    return new GenericListCollectionType();
                if (genType == typeof(HashSet<>))
                    return new HashSetCollectionType();
                if (genType == typeof(IDictionary<,>) || genType == typeof(Dictionary<,>) && type.GetGenericArguments()[0] == typeof(string))
                    return new GenericStringDictionaryCollectionType();
            }

            throw new NotSupportedException(string.Format("Could not create collection type from {0}.", type));
        }

        private Type GetElementType(Type type)
        {
            if (type.IsGenericType)
            {
                var genType = type.GetGenericTypeDefinition();
                if (genType == typeof(IList<>) || genType == typeof(List<>) || genType == typeof(ICollection<>) || genType == typeof(HashSet<>))
                    return type.GetGenericArguments()[0];
                if (genType == typeof(IDictionary<,>) || genType == typeof(Dictionary<,>) && type.GetGenericArguments()[0] == typeof(string))
                    return type.GetGenericArguments()[1];
            }

            throw new NotSupportedException(string.Format("Could not discover element type from {0}.", type));
        }

        private bool IsCollection(Type type)
        {
            if (type.IsGenericType)
            {
                var genType = type.GetGenericTypeDefinition();
                return genType == typeof(IList<>)
                    || genType == typeof(List<>)
                    || genType == typeof(ICollection<>)
                    || genType == typeof(HashSet<>)
                    || ((genType == typeof(IDictionary<,>) || genType == typeof(Dictionary<,>)) && type.GetGenericArguments()[0] == typeof(string));
            }

            return false;
        }

        private IValueConverter GetValueConverter(Type type)
        {
            return new NullSafeValueConverter(type);
        }

        #endregion
    }
}