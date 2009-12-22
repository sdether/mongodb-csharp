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
    public class FindAllAction : PersistenceAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindAllAction"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="changeTracker">The change tracker.</param>
        /// <param name="hydrator">The hydrator.</param>
        /// <param name="collection">The collection.</param>
        public FindAllAction(MappingStore mappingStore, ChangeTracker changeTracker, IEntityHydrator hydrator, IMongoCollection collection)
            : base(mappingStore, changeTracker, hydrator, collection)
        { }

        /// <summary>
        /// Finds this instance.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public IEnumerable<TEntity> Find<TEntity>()
        {
            var cursor = this.Collection.FindAll();
            foreach (var document in cursor.Documents)
                yield return this.Hydrator.HydrateEntity<TEntity>(document);
        }
    }
}