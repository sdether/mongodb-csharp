using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Framework.Mapping;
using System.Text.RegularExpressions;

namespace MongoDB.Framework.Persistence
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
            var documentMap = this.mappingStore.GetDocumentMapFor(entity.GetType());
            return this.CreateDocument(documentMap, entity);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates the document.
        /// </summary>
        /// <param name="documentMap">The document map.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        private Document CreateDocument(DocumentMap documentMap, object entity)
        {
            var document = new Document();
            if (documentMap.HasId)
                this.ApplyIdMap(documentMap.IdMap, entity, document);

            this.ApplySimpleValueMaps(documentMap.SimpleValueMaps, entity, document);
            this.ApplyNestedDocumentValueMaps(documentMap.NestedDocumentValueMaps, entity, document);
            this.ApplyReferenceValueMaps(documentMap.ReferenceValueMaps, entity, document);

            if (documentMap.IsPolymorphic)
                document[documentMap.DiscriminatorKey] = documentMap.Discriminator;

            if(documentMap.HasExtendedProperties)
                this.ApplyExtendedPropertiesMap(documentMap.ExtendedPropertiesMap, entity, document);

            if (documentMap.IsPolymorphic && documentMap.Discriminator != null)
                document[documentMap.DiscriminatorKey] = documentMap.Discriminator;

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
            value = idMap.ConvertToDocumentValue(value);
            if(value != MongoDBNull.Value || idMap.PersistNulls)
                document[idMap.Key] = value;
        }

        /// <summary>
        /// Applies the nested document maps.
        /// </summary>
        /// <param name="nestedDocumentValueMaps">The nested document value maps.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="document">The document.</param>
        private void ApplyNestedDocumentValueMaps(IEnumerable<NestedDocumentValueMap> nestedDocumentValueMaps, object entity, Document document)
        {
            foreach (var nestedDocumentValueMap in nestedDocumentValueMaps)
            {
                var value = nestedDocumentValueMap.MemberGetter(entity);
                value = this.CreateDocument(nestedDocumentValueMap.RootDocumentMap, value);
                document[nestedDocumentValueMap.Key] = value;
            }
        }

        /// <summary>
        /// Applies the nested document maps.
        /// </summary>
        /// <param name="nestedDocumentValueMaps">The nested document value maps.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="document">The document.</param>
        private void ApplyReferenceValueMaps(IEnumerable<ReferenceValueMap> referenceValueMaps, object entity, Document document)
        {
            foreach (var referenceValueMap in referenceValueMaps)
                throw new NotSupportedException("ReferenceValueMaps are not supported.");
        }

        /// <summary>
        /// Applies the simple value maps.
        /// </summary>
        /// <param name="simpleValueMaps">The simple value maps.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="document">The document.</param>
        private void ApplySimpleValueMaps(IEnumerable<SimpleValueMap> simpleValueMaps, object entity, Document document)
        {
            foreach (var simpleValueMap in simpleValueMaps)
            {
                object value = simpleValueMap.MemberGetter(entity);
                value = simpleValueMap.ConvertToDocumentValue(value);
                if(value != MongoDBNull.Value || simpleValueMap.PersistNulls)
                    document[simpleValueMap.Key] = value;
            }
        }

        #endregion
    }
}