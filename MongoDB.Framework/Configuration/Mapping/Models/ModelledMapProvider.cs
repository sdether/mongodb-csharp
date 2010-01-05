using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using MongoDB.Framework.Mapping;
using MongoDB.Framework.Mapping.IdGenerators;
using MongoDB.Framework.Mapping.Types;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Models
{
    public class ModelledMapProvider : IMapProvider
    {
        #region Private Fields

        private Dictionary<Type, Func<Type, Type>> elementTypeFactories = new Dictionary<Type, Func<Type, Type>>()        {            { typeof(ICollection<>), mt => mt.GetGenericArguments()[0] },            { typeof(IList<>), mt => mt.GetGenericArguments()[0] },            { typeof(List<>), mt => mt.GetGenericArguments()[0] },            { typeof(HashSet<>), mt => mt.GetGenericArguments()[0] },            { typeof(IDictionary<,>), mt => mt.GetGenericArguments()[1] },            { typeof(Dictionary<,>), mt => mt.GetGenericArguments()[1] },        };

        private Dictionary<Type, RootClassMapModel> rootClassMapModels;
        private Dictionary<Type, NestedClassMapModel> nestedClassMapModels;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelledMapProvider"/> class.
        /// </summary>
        public ModelledMapProvider()
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
        /// Gets the root class map for.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// The RootClassMap if it exists; otherwise <c>null</c>.
        /// </returns>
        public RootClassMap GetRootClassMapFor(Type type)
        {
            RootClassMapModel rootClassMapModel;
            if (!this.rootClassMapModels.TryGetValue(type, out rootClassMapModel))
                return null;

            return this.BuildRootClassMap(rootClassMapModel);
        }

        /// <summary>
        /// Gets the nested class map for.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public NestedClassMap GetNestedClassMapFor(Type type)
        {
            NestedClassMapModel nestedClassMapModel;
            if (!this.nestedClassMapModels.TryGetValue(type, out nestedClassMapModel))
                return null;

            return this.BuildNestedClassMap(nestedClassMapModel);
        }

        #endregion

        #region Private Methods

        private RootClassMap BuildRootClassMap(RootClassMapModel model)
        {
            var manyToOneMaps = model.ManyToOneMaps.Select(mto => this.BuildManyToOneMap(mto)).ToList();
            var memberMaps = model.ValueMaps.Select(vm => this.BuildMemberMap(vm))
                .Concat(model.CollectionMaps.Select(cm => this.BuildMemberMap(cm)))
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
                manyToOneMaps,
                model.DiscriminatorKey,
                model.Discriminator,
                subClassMaps,
                extPropMap,
                indexes);

            return rootClassMap;
        }

        private NestedClassMap BuildNestedClassMap(NestedClassMapModel model)
        {
            var manyToOneMaps = model.ManyToOneMaps.Select(mto => this.BuildManyToOneMap(mto)).ToList();
            var memberMaps = model.ValueMaps.Select(vm => this.BuildMemberMap(vm))
                            .Concat(model.CollectionMaps.Select(cm => this.BuildMemberMap(cm)))
                            .ToList();
            var extPropMap = this.BuildExtendedPropertiesMap(model.ExtendedPropertiesMap);

            var subClassMaps = model.SubClassMaps.Select(sc => this.BuildSubClassMap(sc));

            var nestedClassMap = new NestedClassMap(
                model.Type,
                memberMaps,
                manyToOneMaps,
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
            IValueType valueType = null;
            IIdGenerator generator = model.Generator;
            object unsavedValue = model.UnsavedValue;

            var memberType = ReflectionUtil.GetMemberValueType(model.Getter);
            if (memberType == typeof(string) && generator == null)
            {
                if (generator == null)
                {
                    generator = new OidGenerator();
                    if (valueType == null)
                        valueType = new OidValueType();
                }
            }
            else if (memberType == typeof(Guid) && generator == null)
                generator = new GuidGenerator();

            if (valueType == null)
                valueType = this.GetValueTypeFromType(memberType);

            if (unsavedValue == null)
                unsavedValue = memberType.IsValueType ? Activator.CreateInstance(memberType) : null;

            return new IdMap(model.Getter.Name, getter, setter, valueType, generator, unsavedValue);
        }

        private Index BuildIndex(IndexModel model)
        {
            if (model.Name == null)
                model.Name = model.Parts.Aggregate("ix", (c, p) => c + "_" + p.Key);

            return new Index(model.Name, model.Parts, model.IsUnique);
        }

        private ManyToOneMap BuildManyToOneMap(ManyToOneMapModel model)
        {
            var getter = LateBoundReflection.GetGetter(model.Getter);
            var setter = LateBoundReflection.GetSetter(model.Setter);
            string name = model.Getter.Name;
            string key = model.Key ?? name;
            var memberValueType = ReflectionUtil.GetMemberValueType(model.Getter);
            bool isLazy = model.IsLazy;

            if (memberValueType.IsSealed)
                isLazy = false;
            
            return new ManyToOneMap(key, name, getter, setter, memberValueType, isLazy);
        }

        private MemberMap BuildMemberMap(MemberMapModelBase model)
        {
            var getter = LateBoundReflection.GetGetter(model.Getter);
            var setter = LateBoundReflection.GetSetter(model.Setter);
            string name = model.Getter.Name;
            string key = model.Key ?? name;
            var memberValueType = ReflectionUtil.GetMemberValueType(model.Getter);
            IValueType valueType;

            if (model is ValueMapModel)
            {
                var value = (ValueMapModel)model;
                valueType = value.CustomValueType ?? this.GetValueTypeFromType(memberValueType);
            }
            else if (model is CollectionMapModel)
            {
                valueType = this.GetCollectionValueType(memberValueType, (CollectionMapModel)model);
            }
            else
                throw new NotSupportedException("Unknown MemberMap.");

            return new MemberMap(
                key,
                name,
                getter,
                setter,
                valueType);
        }

        private SubClassMap BuildSubClassMap(SubClassMapModel model)
        {
            var manyToOneMaps = model.ManyToOneMaps.Select(mto => this.BuildManyToOneMap(mto)).ToList();
            var memberMaps = model.ValueMaps.Select(vm => this.BuildMemberMap(vm))
                            .Concat(model.CollectionMaps.Select(cm => this.BuildMemberMap(cm)))
                            .ToList();
            return new SubClassMap(
                model.Type,
                memberMaps,
                manyToOneMaps,
                model.Discriminator);
        }

        private bool IsCollection(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string);
        }

        private IValueType GetValueTypeFromType(Type type)
        {
            var nestedClassMap = this.GetNestedClassMapFor(type);
            if (nestedClassMap != null)
                return new NestedClassValueType(nestedClassMap);

            if (type == typeof(Guid))
                return new GuidValueType();

            if (type == typeof(Regex))
            {
                //TODO: create a regex value type
            }

            return new NullSafeValueType(type);
        }

        private IValueType GetCollectionValueType(Type memberType, CollectionMapModel model)
        {
            if (model.CollectionType == null)
                model.CollectionType = this.GetCollectionType(memberType);

            if (model.ElementValueType == null)
            {
                if (model.ElementType == null)
                    model.ElementType = this.DiscoverCollectionElementType(memberType);

                model.ElementValueType = this.GetValueTypeFromType(model.ElementType);
            }

            return new CollectionValueType(model.CollectionType, model.ElementValueType);
        }

        private Type DiscoverCollectionElementType(Type memberType)
        {
            if (memberType.IsGenericType)
            {
                Func<Type, Type> elementTypeFactory;
                if (this.elementTypeFactories.TryGetValue(memberType.GetGenericTypeDefinition(), out elementTypeFactory))
                    return elementTypeFactory(memberType);
            }

            throw new NotSupportedException(string.Format("Could not discover element type from {0}.", memberType));
        }

        private ICollectionType GetCollectionType(Type memberType)
        {
            if (memberType.IsGenericType)
            {
                var genType = memberType.GetGenericTypeDefinition();
                if (genType == typeof(IList<>) || genType == typeof(List<>) || genType == typeof(ICollection<>))
                    return new ListCollectionType();
                if (genType == typeof(HashSet<>))
                    return new SetCollectionType();
                if (genType == typeof(IDictionary<,>) || genType == typeof(Dictionary<,>))
                    return new DictionaryCollectionType();
            }

            throw new NotSupportedException(string.Format("Could not create collection type from {0}.", memberType));
        }


        #endregion
    }
}