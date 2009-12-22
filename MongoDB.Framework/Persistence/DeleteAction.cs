using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Tracking;

namespace MongoDB.Framework.Persistence
{
    public class DeleteAction : PersistenceAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteAction"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="changeTracker">The change tracker.</param>
        /// <param name="collection">The collection.</param>
        public DeleteAction(MappingStore mappingStore, ChangeTracker changeTracker, IMongoCollection collection)
            : base(mappingStore, changeTracker, collection)
        { }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Delete(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var documentMap = this.MappingStore.GetDocumentMapFor(entity.GetType());
            if (!documentMap.HasId)
                throw new InvalidOperationException("Only entities with identifiers are persistable.");

            var document = new Document();
            document[documentMap.IdMap.Key] = MongoTypeConverter.ConvertToOid((string)documentMap.IdMap.MemberGetter(entity));
            this.Collection.Delete(document);
            this.ChangeTracker.GetTrackedObject(entity).MoveToDead();
        }
    }
}