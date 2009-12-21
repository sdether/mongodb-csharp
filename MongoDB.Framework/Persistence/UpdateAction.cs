using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Cache;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Hydration;

namespace MongoDB.Framework.Persistence
{
    public class UpdateAction : PersistenceAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAction"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="sessionLevelCache"></param>
        /// <param name="hydrator">The hydrator.</param>
        /// <param name="mongoCollection">The mongo collection.</param>
        public UpdateAction(MappingStore mappingStore, IEntityCache sessionLevelCache, IEntityHydrator hydrator, IMongoCollection mongoCollection)
            : base(mappingStore, sessionLevelCache, hydrator, mongoCollection)
        { }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Update<TEntity>(TEntity entity)
        {
            var documentMap = this.MappingStore.GetDocumentMapFor<TEntity>();
            if (!documentMap.HasId)
                throw new InvalidOperationException("Only entities with identifiers are persistable.");

            var document = new EntityToDocumentTranslator(this.MappingStore)
                .Translate(entity);
            this.Collection.Update(document);
        }
    }
}