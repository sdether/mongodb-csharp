using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Cache;
using MongoDB.Framework.Hydration;
using MongoDB.Framework.Mapping;

namespace MongoDB.Framework.Persistence
{
    public class GetByIdAction : PersistenceAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdAction"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="sessionLevelCache">The session level cache.</param>
        /// <param name="hydrator">The hydrator.</param>
        /// <param name="mongoCollection">The mongo collection.</param>
        /// <param name="query">The query.</param>
        public GetByIdAction(MappingStore mappingStore, IEntityCache sessionLevelCache, IEntityHydrator hydrator, IMongoCollection mongoCollection)
            : base(mappingStore, sessionLevelCache, hydrator, mongoCollection)
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

            object entity = null;
            if (this.SessionLevelCache.TryToFind(id, out entity))
                return (TEntity)entity;

            var document = new Document();
            document[documentMap.IdMap.Key] = documentMap.IdMap.ConvertToDocumentValue(id);

            document = this.Collection.FindOne(document);
            return this.Hydrator.HydrateEntity<TEntity>(document);
        }
    }
}