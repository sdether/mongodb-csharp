using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Tracking;

namespace MongoDB.Framework.Persistence
{
    public abstract class FindActionBase : PersistenceAction
    {
        #region Private Static Methods

        /// <summary>
        /// Determines whether the query is an identity query.
        /// </summary>
        /// <param name="documentMap">The document map.</param>
        /// <param name="conditions">The conditions.</param>
        /// <returns>
        /// 	<c>true</c> if [is find by id] [the specified document map]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsFindById(DocumentMap documentMap, Document conditions)
        {
            return documentMap.HasId && conditions.Count == 1 && conditions[documentMap.IdMap.Key] != null;
        }

        /// <summary>
        /// Determines whether the query can be called without a cursor.
        /// </summary>
        /// <param name="documentMap">The document map.</param>
        /// <param name="conditions">The conditions.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns>
        /// 	<c>true</c> if [is find one] [the specified document map]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsFindOne(int limit, int skip, Document orderBy)
        {
            return orderBy.Count == 0 && limit == 1 && skip == 0;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FindAction"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="changeTracker">The change tracker.</param>
        /// <param name="collection">The collection.</param>
        public FindActionBase(MappingStore mappingStore, ChangeTracker changeTracker, IMongoCollection collection)
            : base(mappingStore, changeTracker, collection)
        {  }

        #endregion

        #region Protected Methods

        protected IEnumerable<object> Find(DocumentMap documentMap, Document conditions, int limit, int skip, Document orderBy, Document fields)
        {
            if (documentMap == null)
                throw new ArgumentNullException("documentMap");
            if (conditions == null)
                throw new ArgumentNullException("conditions");
            if (orderBy == null)
                throw new ArgumentNullException("orderBy");
            if (fields == null)
                throw new ArgumentNullException("fields");

            var query = this.CreateQuery(conditions, orderBy);
            conditions = (Document)query["query"] ?? query;

            IEnumerable<Document> documents;
            if (IsFindById(documentMap, conditions))
            {
                string id = (string)MongoTypeConverter.ConvertFromDocumentValue(conditions[documentMap.IdMap.Key]);
                TrackedObject trackedObject = null;
                if (this.ChangeTracker.TryGetTrackedObjectById(id, out trackedObject))
                    return new[] { trackedObject.Current };

                documents = new[] { this.Collection.FindOne(conditions) };
            }
            else if (IsFindOne(limit, skip, orderBy))
            {
                //if the particular entity type we need has a discriminator, we need to filter on it...
                if (documentMap.IsPolymorphic && documentMap.Discriminator != null)
                    conditions[documentMap.DiscriminatorKey] = documentMap.Discriminator;
                var document = this.Collection.FindOne(conditions);
                if (document == null)
                    documents = Enumerable.Empty<Document>();
                else
                    documents = new[] {  document };
            }
            else
            {
                if (documentMap.IsPolymorphic)
                {
                    //if we are projecting, we need to make sure we get the discriminator back as well...
                    if (fields.Count != 0)
                        fields[documentMap.DiscriminatorKey] = 1;

                    //if the particular entity type we need has a discriminator, we need to filter on it...
                    if (documentMap.Discriminator != null)
                        conditions[documentMap.DiscriminatorKey] = documentMap.Discriminator;
                }

                documents = this.Collection.Find(query, limit, skip, fields).Documents;
            }

            //don't track entities returned from a projection
            DocumentToEntityTranslator translator;
            if (fields.Count == 0)
                translator = new ChangeTrackingDocumentToEntityTranslator(this.MappingStore, this.ChangeTracker);
            else
                translator = new DocumentToEntityTranslator(this.MappingStore);
            return this.CreateEntities(translator, documentMap, documents);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates the entities.
        /// </summary>
        /// <param name="translator">The translator.</param>
        /// <param name="documentMap">The document map.</param>
        /// <param name="documents">The documents.</param>
        /// <returns></returns>
        private IEnumerable<object> CreateEntities(DocumentToEntityTranslator translator, DocumentMap documentMap, IEnumerable<Document> documents)
        {
            foreach (var document in documents)
            {
                if (document == null)
                    yield return null;
                else
                    yield return this.CreateEntity(translator, documentMap, document);
            }
        }

        /// <summary>
        /// Creates the entity.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        private object CreateEntity(DocumentToEntityTranslator translator, DocumentMap documentMap, Document document)
        {
            return translator.Translate(documentMap, document);
        }

        /// <summary>
        /// Creates the full query.
        /// </summary>
        /// <returns></returns>
        private Document CreateQuery(Document conditions, Document orderBy)
        {
            if (orderBy.Count == 0)
                return conditions;

            return new Document()
                .Append("query", conditions)
                .Append("orderby", orderBy);
        }

        #endregion

        #region Private Class : ChangeTrackingDocumentToEntityTranslator

        private class ChangeTrackingDocumentToEntityTranslator : DocumentToEntityTranslator
        {
            private ChangeTracker changeTracker;

            /// <summary>
            /// Initializes a new instance of the <see cref="ChangeTrackingDocumentToEntityTranslator"/> class.
            /// </summary>
            /// <param name="mappingStore">The mapping store.</param>
            /// <param name="changeTracker">The change tracker.</param>
            public ChangeTrackingDocumentToEntityTranslator(MappingStore mappingStore, ChangeTracker changeTracker)
                : base(mappingStore)
            {
                this.changeTracker = changeTracker;
            }

            /// <summary>
            /// Translates the specified document map.
            /// </summary>
            /// <param name="documentMap">The document map.</param>
            /// <param name="document">The document.</param>
            /// <returns></returns>
            public override object Translate(DocumentMap documentMap, Document document)
            {
                if (documentMap.HasId)
                {
                    var value = document[documentMap.IdMap.Key];
                    value = MongoTypeConverter.ConvertFromDocumentValue(value);
                    TrackedObject trackedObject;
                    if (this.changeTracker.TryGetTrackedObjectById((string)value, out trackedObject))
                        return trackedObject.Current;
                }

                var entity = base.Translate(documentMap, document);

                if (documentMap.HasId)
                    this.changeTracker.Track(document, entity);

                //TODO: may need to fix up references...
                return entity;
            }
        }

        #endregion
    }
}