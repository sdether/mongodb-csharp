using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Hydration;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Tracking;

namespace MongoDB.Framework.Persistence
{
    public class GetByIdAction : PersistenceAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdAction"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="changeTracker">The change tracker.</param>
        /// <param name="hydrator">The hydrator.</param>
        /// <param name="collection">The collection.</param>
        public GetByIdAction(MappingStore mappingStore, ChangeTracker changeTracker, IEntityHydrator hydrator, IMongoCollection collection)
            : base(mappingStore, changeTracker, hydrator, collection)
        { }

        /// <summary>
        /// Finds the one.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public TEntity GetById<TEntity>(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Cannot be null or empty.", "id");

            var documentMap = this.MappingStore.GetDocumentMapFor<TEntity>();
            if (!documentMap.HasId)
                throw new InvalidOperationException("Only entities with identifiers are persistable.");

            TrackedObject trackedObject = null;
            if (this.ChangeTracker.TryGetTrackedObjectById(id, out trackedObject))
                return (TEntity)trackedObject.Current;

            var document = new Document();
            document[documentMap.IdMap.Key] = documentMap.IdMap.ConvertToDocumentValue(id);

            document = this.Collection.FindOne(document);
            return this.Hydrator.HydrateEntity<TEntity>(document);
        }
    }
}