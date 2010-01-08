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
using MongoDB.Framework.Mapping.Converters;
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

        private Dictionary<Type, RootClassMap> rootClassMaps;

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
            foreach (var subClassMapModel in rootClassMapModel.SubClassMaps)
                this.rootClassMapModels.Add(subClassMapModel.Type, rootClassMapModel);
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

        private void BuildRootClassMaps()
        {
            this.rootClassMaps = new Dictionary<Type, RootClassMap>();

            foreach (var rootClassMapModel in this.rootClassMapModels.Values)
            {
                var rootClassMap = this.BuildRootClassMap(rootClassMapModel);
                this.rootClassMaps[rootClassMap.Type] = rootClassMap;
            }
        }

        private RootClassMap BuildRootClassMap(RootClassMapModel model)
        {
            var memberMaps = model.ValueMaps.Select(v => this.BuildMemberMap(v))
                .Concat(model.CollectionMaps.Select(c => this.BuildMemberMap(c)))
                .Concat(model.ManyToOneMaps.Select(mto => this.BuildMemberMap(mto)))
                .ToList();

            var extPropMap = this.BuildExtendedPropertiesMap(model.ExtendedPropertiesMap);
            var idMap = this.BuildIdMap(model.IdMap);
            string collectionName = model.CollectionName ?? model.Type.Name;

            var subClassMaps = model.SubClassMaps.Select(sc => this.BuildSubClassMap(sc));

            var indexes = model.Indexes.Select(i => this.BuildIndex(i));

            var rootClassMap = new RootClassMap(
                model.Type,
                collectionName,
                idMap,
                memberMaps,
                model.DiscriminatorKey,
                model.Discriminator,
                subClassMaps,
                extPropMap,
                indexes);

            return rootClassMap;
        }

        private NestedClassMap BuildNestedClassMap(NestedClassMapModel model)
        {
            var memberMaps = model.ValueMaps.Select(v => this.BuildMemberMap(v))
                            .Concat(model.CollectionMaps.Select(c => this.BuildMemberMap(c)))
                            .Concat(model.ManyToOneMaps.Select(mto => this.BuildMemberMap(mto)))
                            .ToList(); 
            
            var extPropMap = this.BuildExtendedPropertiesMap(model.ExtendedPropertiesMap);
            IdMap idMap = null;
            if(model.IdMap != null)
                idMap = this.BuildIdMap(model.IdMap);
            var subClassMaps = model.SubClassMaps.Select(sc => this.BuildSubClassMap(sc));

            var nestedClassMap = new NestedClassMap(
                model.Type,
                idMap,
                memberMaps,
                model.DiscriminatorKey,
                model.Discriminator,
                subClassMaps,
                extPropMap);

            return nestedClassMap;
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

        private MemberMap BuildMemberMap(MemberMapModelBase model)
        {
            var getter = LateBoundReflection.GetGetter(model.Getter);
            var setter = LateBoundReflection.GetSetter(model.Setter);
            string name = model.Getter.Name;
            string key = model.Key ?? name;
            var memberValueType = ReflectionUtil.GetMemberValueType(model.Getter);
            var persistNull = model.PersistNull;

            ValueTypeBase valueType = null;

            if (model is MemberMapModel)
            {
                var memberMapModel = (MemberMapModel)model;
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

            return new MemberMap(
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
            
            return new SubClassMap(
                model.Type,
                memberMaps,
                model.Discriminator);
        }

        private bool IsCollection(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string);
        }

        private NestedClassMap GetNestedClassMapFor(Type type)
        {
            NestedClassMapModel nestedClassMapModel;
            if (!this.nestedClassMapModels.TryGetValue(type, out nestedClassMapModel))
                return null;

            return this.BuildNestedClassMap(nestedClassMapModel);
        }

        private ValueTypeBase GetValueTypeForType(Type type)
        {
            NestedClassMapModel nestedClassMapModel = null;
            if (this.nestedClassMapModels.TryGetValue(type, out nestedClassMapModel))
                return new NestedClassValueType(this.BuildNestedClassMap(nestedClassMapModel));

            return new SimpleValueType(type, this.GetValueConverterForType(type));
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