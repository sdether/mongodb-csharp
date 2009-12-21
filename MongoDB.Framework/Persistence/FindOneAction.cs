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
    public class FindOneAction : PersistenceAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindOneAction"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="changeTracker">The change tracker.</param>
        /// <param name="hydrator">The hydrator.</param>
        /// <param name="mongoCollection">The mongo collection.</param>
        public FindOneAction(MappingStore mappingStore, ChangeTracker changeTracker, IEntityHydrator hydrator, IMongoCollection mongoCollection)
            : base(mappingStore, changeTracker, hydrator, mongoCollection)
        { }

        /// <summary>
        /// Finds the one.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public TEntity FindOne<TEntity>(Document query)
        {
            var document = this.Collection.FindOne(query);
            return this.Hydrator.HydrateEntity<TEntity>(document);
        }
    }
}