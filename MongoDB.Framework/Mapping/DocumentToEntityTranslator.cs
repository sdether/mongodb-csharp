using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping
{
    public class DocumentToEntityTranslator
    {
        #region Private Fields

        private MappingStore mappingStore;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentToEntityTranslator"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        public DocumentToEntityTranslator(MappingStore mappingStore)
        {
            if (mappingStore == null)
                throw new ArgumentNullException("mappingStore");

            this.mappingStore = mappingStore;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Translates the specified entity type.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public object Translate(Type entityType, Document document)
        {
            var documentMap = this.mappingStore.GetDocumentMapFor(entityType);
            return this.Translate(documentMap, document);
        }

        /// <summary>
        /// Translates the specified document map.
        /// </summary>
        /// <param name="documentMap">The document map.</param>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public virtual object Translate(DocumentMap documentMap, Document document)
        {
            //this is a destructive process in order to obtain extended properties.
            //therefore we will work off of a copy in order to preserve the original document.
            var documentCopy = new Document();
            document.CopyTo(documentCopy); 

            if(documentMap.IsPolymorphic)
            {
                var discriminator = document[documentMap.DiscriminatorKey];
                //TODO: potentially allow for conversion here...
                documentMap = documentMap.GetDocumentMapByDiscriminator(discriminator);
            }

            var entity = Activator.CreateInstance(documentMap.EntityType);
            this.Translate(documentMap, entity, documentCopy);
            return entity;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Hydrates the specified entity map.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entityMap">The entity map.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="document">The document.</param>
        private void Translate(DocumentMap documentMap, object entity, Document document)
        {
            if (documentMap.HasId)
                this.ApplyIdMap(documentMap.IdMap, entity, document);

            this.ApplySimpleValueMaps(documentMap.SimpleValueMaps, entity, document);
            this.ApplyNestedDocumentValueMaps(documentMap.NestedDocumentValueMaps, entity, document);
            this.ApplyReferenceValueMaps(documentMap.ReferenceValueMaps, entity, document);

            if (documentMap.IsPolymorphic)
                document.Remove(documentMap.DiscriminatorKey);

            if(documentMap.HasExtendedProperties)
                this.ApplyExtendedPropertiesMap(documentMap.ExtendedPropertiesMap, entity, document);
        }

        /// <summary>
        /// Applies the extended properties map.
        /// </summary>
        /// <param name="extendedPropertiesMap">The extended properties map.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="document">The document.</param>
        private void ApplyExtendedPropertiesMap(ExtendedPropertiesMap extendedPropertiesMap, object entity, Document document)
        {
            var dictionary = document.ToDictionary();
            extendedPropertiesMap.MemberSetter(entity, dictionary);
        }

        /// <summary>
        /// Applies the id map.
        /// </summary>
        /// <param name="idMap">The id map.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="document">The document.</param>
        private void ApplyIdMap(IdMap idMap, object entity, Document document)
        {
            var value = document[idMap.Key];
            document.Remove(idMap.Key);
            value = MongoTypeConverter.ConvertFromDocumentValue(value);
            idMap.MemberSetter(entity, value);
        }

        /// <summary>
        /// Applies the nested document value maps.
        /// </summary>
        /// <param name="nestedDocumentValueMaps">The nested document value maps.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="document">The document.</param>
        private void ApplyNestedDocumentValueMaps(IEnumerable<NestedDocumentValueMap> nestedDocumentValueMaps, object entity, Document document)
        {
            foreach (var nestedDocumentMap in nestedDocumentValueMaps)
            {
                var value = document[nestedDocumentMap.Key] as Document;
                document.Remove(nestedDocumentMap.Key);
                if (value == null)
                    return;

                var nestedEntity = this.Translate(nestedDocumentMap.RootDocumentMap, value);
                nestedDocumentMap.MemberSetter(entity, nestedEntity);
            }
        }

        /// <summary>
        /// Applies the reference value maps.
        /// </summary>
        /// <param name="referenceValueMaps">The reference value maps.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="document">The document.</param>
        private void ApplyReferenceValueMaps(IEnumerable<ReferenceValueMap> referenceValueMaps, object entity, Document document)
        {
            foreach (var referenceValueMap in referenceValueMaps)
                throw new NotSupportedException("ReferenceValueMaps are not supported yet.");
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
                var value = document[simpleValueMap.Key];
                document.Remove(simpleValueMap.Key);
                value = MongoTypeConverter.ConvertFromDocumentValue(value);
                if (value == null)
                    value = GetDefaultValue(simpleValueMap.MemberType);
                simpleValueMap.MemberSetter(entity, value);
            }
        }

        private static object GetDefaultValue(Type entityType)
        {
            if (entityType.IsValueType)
                return Activator.CreateInstance(entityType);
            return null;
        }

        #endregion
    }
}