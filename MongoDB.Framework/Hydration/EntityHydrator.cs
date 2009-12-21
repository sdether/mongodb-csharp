using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Tracking;

namespace MongoDB.Framework.Hydration
{
    public class EntityHydrator : IEntityHydrator
    {
        #region Private Fields

        private ChangeTracker changeTracker;
        private MappingStore mappingStore;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityHydrator"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="changeTracker">The change tracker.</param>
        public EntityHydrator(MappingStore mappingStore, ChangeTracker changeTracker)
        {
            if (mappingStore == null)
                throw new ArgumentNullException("mappingStore");
            if (changeTracker == null)
                throw new ArgumentNullException("changeTracker");

            this.changeTracker = changeTracker;
            this.mappingStore = mappingStore;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Hydrates the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public TEntity HydrateEntity<TEntity>(Document document)
        {
            var documentMap = this.mappingStore.GetDocumentMapFor<TEntity>();
            object entity = null;
            string id = null;
            if (documentMap.HasId)
            {
                var value = document[documentMap.IdMap.Key];
                id = (string)documentMap.IdMap.ConvertFromDocumentValue(value);
                TrackedObject trackedObject;
                if (id != null && this.changeTracker.TryGetTrackedObjectById(id, out trackedObject))
                    return (TEntity)trackedObject.Current;
            }

            entity = this.CreateEntityFromDocument(documentMap, document);

            if (id != null)
                this.changeTracker.Track(document, entity);
            return (TEntity)entity;
        }

        /// <summary>
        /// Hydrates the entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="documents">The documents.</param>
        /// <returns></returns>
        public IEnumerable<TEntity> HydrateEntities<TEntity>(IEnumerable<Document> documents)
        {
            foreach (var document in documents)
                yield return this.HydrateEntity<TEntity>(document);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates the entity from document.
        /// </summary>
        /// <param name="documentMap">The document map.</param>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        private object CreateEntityFromDocument(DocumentMap documentMap, Document document)
        {
            var entity = Activator.CreateInstance(documentMap.EntityType);
            this.Hydrate(documentMap, entity, document);
            return entity;
        }

        /// <summary>
        /// Hydrates the specified entity map.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entityMap">The entity map.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="document">The document.</param>
        private void Hydrate(DocumentMap documentMap, object entity, Document document)
        {
            this.ApplySimpleValueMaps(documentMap.SimpleValueMaps, entity, document);
            this.ApplyNestedDocumentValueMaps(documentMap.NestedDocumentValueMaps, entity, document);
            this.ApplyReferenceValueMaps(documentMap.ReferenceValueMaps, entity, document);

            if (documentMap.IsPolymorphic)
                document.Remove(documentMap.DiscriminatorKey);

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
            if (extendedPropertiesMap == null)
                return;

            var dictionary = document.ToDictionary();
            extendedPropertiesMap.MemberSetter(entity, dictionary);
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

                var nestedEntity = this.CreateEntityFromDocument(nestedDocumentMap.RootDocumentMap, value);
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
                value = simpleValueMap.ConvertFromDocumentValue(value);
                simpleValueMap.MemberSetter(entity, value);
            }
        }

        #endregion
    }
}