using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Reflection;
using MongoDB.Framework.Mapping.Types;
using System.Text.RegularExpressions;
using MongoDB.Framework.Mapping.IdGenerators;
using System.Collections;

namespace MongoDB.Framework.Mapping.Models
{
    public class ModelledMapProvider : IMapProvider
    {
        #region Private Fields

        private Dictionary<Type, Func<Type, Type>> elementTypeFactories = new Dictionary<Type, Func<Type, Type>>()
        {
            { typeof(ICollection<>), mt => mt.GetGenericArguments()[0] },
            { typeof(IList<>), mt => mt.GetGenericArguments()[0] },
            { typeof(List<>), mt => mt.GetGenericArguments()[0] },
            { typeof(HashSet<>), mt => mt.GetGenericArguments()[0] },
            { typeof(IDictionary<,>), mt => mt.GetGenericArguments()[1] },
            { typeof(Dictionary<,>), mt => mt.GetGenericArguments()[1] },
        };

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
            var memberMaps = new List<MemberMap>();
            memberMaps.AddRange(model.MemberMaps.Select(mm => this.BuildMemberMap(mm)));
            var extPropMap = this.BuildExtendedPropertiesMap(model.ExtendedPropertiesMap);
            var idMap = this.BuildIdMap(model.IdMap);
            string collectionName = model.CollectionName ?? model.Type.Name;

            var subClassMaps = model.SubClassMaps.Select(sc => this.BuildSubClassMap(sc));

            var rootClassMap = new RootClassMap(
                model.Type,
                collectionName,
                idMap,
                memberMaps,
                model.DiscriminatorKey,
                model.Discriminator,
                subClassMaps,
                extPropMap);

            return rootClassMap;
        }

        private NestedClassMap BuildNestedClassMap(NestedClassMapModel model)
        {
            var memberMaps = new List<MemberMap>();
            memberMaps.AddRange(model.MemberMaps.Select(mm => this.BuildMemberMap(mm)));
            var extPropMap = this.BuildExtendedPropertiesMap(model.ExtendedPropertiesMap);

            var subClassMaps = model.SubClassMaps.Select(sc => this.BuildSubClassMap(sc));

            var nestedClassMap = new NestedClassMap(
                model.Type,
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
            IValueType valueType = null;
            if(model.Part != null)
                valueType = ((EmbeddedValuePart)model.Part).CustomValueType;
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

        private MemberMap BuildMemberMap(MemberMapModel model)
        {
            var getter = LateBoundReflection.GetGetter(model.Getter);
            var setter = LateBoundReflection.GetSetter(model.Setter);
            string name = model.Getter.Name;
            string key = model.Key ?? name;
            var memberValueType = ReflectionUtil.GetMemberValueType(model.Getter);
            IValueType valueType;

            if (model.Part == null)
            {
                if (this.IsCollection(memberValueType))
                    model.Part = new EmbeddedCollectionPart();
                else //this will also pick up nested class...
                    model.Part = new EmbeddedValuePart();
            }

            if (model.Part is EmbeddedValuePart)
            {
                var valuePart = (EmbeddedValuePart)model.Part;
                valueType = valuePart.CustomValueType ?? this.GetValueTypeFromType(memberValueType);
            }
            else if (model.Part is EmbeddedCollectionPart)
            {
                valueType = this.GetCollectionValueType(memberValueType, (EmbeddedCollectionPart)model.Part);
            }
            else if (model.Part is EmbeddedClassPart)
            {
                var ncPart = (EmbeddedClassPart)model.Part;
                valueType = new NestedClassValueType(this.BuildNestedClassMap(ncPart.NestedClassMap));
            }
            else
                throw new NotSupportedException("Unknown EmbeddedMemberPart.");

            return new MemberMap(
                key,
                name,
                getter,
                setter,
                valueType);
        }

        private SubClassMap BuildSubClassMap(SubClassMapModel model)
        {
            var subClassMemberMaps = model.MemberMaps.Select(mm => this.BuildMemberMap(mm));
            return new SubClassMap(
                model.Type,
                subClassMemberMaps,
                model.Discriminator);
        }

        private bool IsCollection(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string);
        }

        private IValueType GetValueTypeFromType(Type type)
        {
            NestedClassMapModel nestedClassMapModel;            if (this.nestedClassMapModels.TryGetValue(type, out nestedClassMapModel))            {                return new NestedClassValueType(                    this.BuildNestedClassMap(nestedClassMapModel));            }

            if (type == typeof(Guid))
                return new GuidValueType();

            if (type == typeof(Regex))
            {
                //TODO: create a regex value type
            }

            return new NullSafeValueType(type);
        }

        private IValueType GetCollectionValueType(Type memberType, EmbeddedCollectionPart collectionPart)
        {
            if (collectionPart.CollectionType == null)
                collectionPart.CollectionType = this.GetCollectionType(memberType);

            if (collectionPart.ElementValueType == null)
            {
                if (collectionPart.ElementType == null)
                    collectionPart.ElementType = this.DiscoverElementType(memberType);
                collectionPart.ElementValueType = this.GetValueTypeFromType(collectionPart.ElementType);
            }

            return new CollectionValueType(collectionPart.CollectionType, collectionPart.ElementValueType);
        }

        private Type DiscoverElementType(Type memberType)
        {
            if (memberType.IsGenericType)
            {
                Func<Type, Type> elementTypeFactory;
                if(this.elementTypeFactories.TryGetValue(memberType.GetGenericTypeDefinition(), out elementTypeFactory))
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