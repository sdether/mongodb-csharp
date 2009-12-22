using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Remotion.Data.Linq;

using MongoDB.Driver;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Tracking;

namespace MongoDB.Framework.Linq
{
    public class MongoQueryable<TEntity> : QueryableBase<TEntity>
    {
        /// <summary>
        /// Creates the executor.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="changeTracker">The change tracker.</param>
        /// <param name="hydrator">The hydrator.</param>
        /// <returns></returns>
        private static IQueryExecutor CreateExecutor(MappingStore mappingStore, ChangeTracker changeTracker, Database database)
        {
            return new MongoQueryExecutor(mappingStore, changeTracker, database);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoQueryable&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="changeTracker">The change tracker.</param>
        /// <param name="hydrator">The hydrator.</param>
        public MongoQueryable(MappingStore mappingStore, ChangeTracker changeTracker, Database database)
            : base(CreateExecutor(mappingStore, changeTracker, database))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoQueryable&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="expression">The expression.</param>
        public MongoQueryable(IQueryProvider provider, Expression expression)
            : base(provider, expression)
        {
        }
    }
}