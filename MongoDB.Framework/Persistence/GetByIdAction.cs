using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Tracking;

namespace MongoDB.Framework.Persistence
{
    public class GetByIdAction : FindActionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdAction"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="changeTracker">The change tracker.</param>
        /// <param name="collection">The collection.</param>
        public GetByIdAction(MappingStore mappingStore, ChangeTracker changeTracker, IMongoCollection collection)
            : base(mappingStore, changeTracker, collection)
        { }

        /// <summary>
        /// Finds the one.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public object GetById(Type entityType, string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Cannot be null or empty.", "id");

            var documentMap = this.MappingStore.GetDocumentMapFor(entityType);
            if (!documentMap.HasId)
                throw new InvalidOperationException("Only entities with identifiers are persistable.");

            TrackedObject trackedObject = null;
            if (this.ChangeTracker.TryGetTrackedObjectById(id, out trackedObject))
                return trackedObject.Current;

            var document = new Document();
            document[documentMap.IdMap.Key] = MongoTypeConverter.ConvertToOid((string)id);

            document = this.Collection.FindOne(document);
            return this.CreateEntity(entityType, document);
        }
    }
}