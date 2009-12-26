using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Framework.Mapping;
using System.Text.RegularExpressions;

namespace MongoDB.Framework.Mapping
{
    public class EntityToDocumentTranslator
    {
        #region Private Static Fields

        private readonly static Dictionary<Type, Func<object, object>> mongoTypeConverters = new Dictionary<Type, Func<object, object>>()
        {
            { typeof(Regex), x => ConvertToMongoRegex((Regex)x) }
        };

        #endregion

        #region Private Static Methods

        private static MongoRegex ConvertToMongoRegex(Regex regex)
        {
            //TODO: handle options...
            return new MongoRegex(regex.ToString());
        }

        /// <summary>
        /// Converts the type of from mongo.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static object ConvertToMongoType(object value)
        {
            if (value == null)
                return MongoDBNull.Value;
            Func<object, object> converter = null;
            if (mongoTypeConverters.TryGetValue(value.GetType(), out converter))
                value = converter(value);
            return value;
        }

        #endregion

        #region Private Fields

        private MappingStore mappingStore;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityToDocumentTranslator"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        public EntityToDocumentTranslator(MappingStore mappingStore)
        {
            if (mappingStore == null)
                throw new ArgumentNullException("mappingStore");

            this.mappingStore = mappingStore;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Translates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public Document Translate(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var classMap = this.mappingStore.GetClassMapFor(entity.GetType());
            return this.Translate(classMap, entity);
        }

        /// <summary>
        /// Translates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public Document Translate(ClassMap classMap, object entity)
        {
            if (classMap == null)
                throw new ArgumentNullException("classMap");
            if (entity == null)
                throw new ArgumentNullException("entity");
            return this.CreateDocument(classMap, entity);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates the document.
        /// </summary>
        /// <param name="classMap">The class map.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        private Document CreateDocument(ClassMap classMap, object entity)
        {
            var document = new Document();
            if (classMap.HasId)
                this.ApplyIdMap(classMap.IdMap, entity, document);

            this.ApplySimpleMemberMaps(classMap.SimpleMemberMaps, entity, document);
            this.ApplyNestedClassMemberMaps(classMap.NestedClassMemberMaps, entity, document);
            this.ApplyReferenceMemberMaps(classMap.ReferenceMemberMaps, entity, document);

            if (classMap.IsPolymorphic && classMap.Discriminator != null)
                document[classMap.DiscriminatorKey] = classMap.Discriminator;

            if(classMap.HasExtendedProperties)
                this.ApplyExtendedPropertiesMap(classMap.ExtendedPropertiesMap, entity, document);

            if (classMap.IsPolymorphic && classMap.Discriminator != null)
                document[classMap.DiscriminatorKey] = classMap.Discriminator;

            return document;
        }

        /// <summary>
        /// Applies the extended properties map.
        /// </summary>
        /// <param name="extendedPropertiesMap">The extended properties map.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="document">The document.</param>
        private void ApplyExtendedPropertiesMap(ExtendedPropertiesMap extendedPropertiesMap, object entity, Document document)
        {
            var extProp = (IDictionary<string, object>)extendedPropertiesMap.MemberGetter(entity);
            extProp
                .ToDocument()
                .CopyTo(document);
        }

        /// <summary>
        /// Applies the id map.
        /// </summary>
        /// <param name="idMap">The id map.</param>
        /// <param name="document">The document.</param>
        /// <param name="entity">The entity.</param>
        private void ApplyIdMap(IdMap idMap, object entity, Document document)
        {
            object value = idMap.MemberGetter(entity);
            value = MongoTypeConverter.ConvertToOid((string)value);
            if(value != MongoDBNull.Value || idMap.PersistNulls)
                document[idMap.Key] = value;
        }

        /// <summary>
        /// Applies the nested class maps.
        /// </summary>
        /// <param name="nestedClassMemberMaps">The nested document member maps.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="document">The document.</param>
        private void ApplyNestedClassMemberMaps(IEnumerable<NestedClassMemberMap> nestedClassMemberMaps, object entity, Document document)
        {
            foreach (var nestedClassMemberMap in nestedClassMemberMaps)
            {
                var value = nestedClassMemberMap.MemberGetter(entity);
                value = this.CreateDocument(nestedClassMemberMap.NestedClassMap, value);
                document[nestedClassMemberMap.Key] = value;
            }
        }

        /// <summary>
        /// Applies the nested class maps.
        /// </summary>
        /// <param name="nestedClassMemberMaps">The nested document member maps.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="document">The document.</param>
        private void ApplyReferenceMemberMaps(IEnumerable<ReferenceMemberMap> referenceMemberMaps, object entity, Document document)
        {
            foreach (var referenceMemberMap in referenceMemberMaps)
                throw new NotSupportedException("ReferenceMemberMaps are not supported.");
        }

        /// <summary>
        /// Applies the simple member maps.
        /// </summary>
        /// <param name="simpleMemberMaps">The simple member maps.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="document">The document.</param>
        private void ApplySimpleMemberMaps(IEnumerable<SimpleMemberMap> simpleMemberMaps, object entity, Document document)
        {
            foreach (var simpleMemberMap in simpleMemberMaps)
            {
                object value = simpleMemberMap.MemberGetter(entity);
                value = MongoTypeConverter.ConvertToDocumentValue(simpleMemberMap.MemberType, value);
                if(value != MongoDBNull.Value || simpleMemberMap.PersistNulls)
                    document[simpleMemberMap.Key] = value;
            }
        }

        #endregion
    }
}