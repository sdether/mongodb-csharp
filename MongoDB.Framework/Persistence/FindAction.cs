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
    public class FindAction : PersistenceAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindAction"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="changeTracker">The change tracker.</param>
        /// <param name="hydrator">The hydrator.</param>
        /// <param name="collection">The collection.</param>
        public FindAction(MappingStore mappingStore, ChangeTracker changeTracker, IEntityHydrator hydrator, IMongoCollection collection)
            : base(mappingStore, changeTracker, hydrator, collection)
        { }

        /// <summary>
        /// Finds the specified query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns></returns>
        public IEnumerable<TEntity> Find<TEntity>(Document query, int limit, int skip, Document fields, Document orderBy)
        {
            var cursor = this.Collection.Find(this.CreateFullQuery(query, orderBy), limit, skip, fields);
            foreach (var document in cursor.Documents)
                yield return this.Hydrator.HydrateEntity<TEntity>(document);
        }

        /// <summary>
        /// Creates the full query.
        /// </summary>
        /// <returns></returns>
        private Document CreateFullQuery(Document query, Document orderBy)
        {
            if (orderBy == null || orderBy.Count == 0)
                return query;

            return new Document()
                .Append("query", query)
                .Append("orderBy", orderBy);
        }
    }
}