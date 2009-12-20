using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using MongoDB.Driver;
using Remotion.Data.Linq;
using MongoDB.Framework.Tracking;
using MongoDB.Framework.Configuration;

namespace MongoDB.Framework.Linq
{
    public class MongoQueryable<TEntity> : QueryableBase<TEntity>
    {
        /// <summary>
        /// Creates the executor.
        /// </summary>
        /// <param name="mongo">The mongo.</param>
        /// <returns></returns>
        private static IQueryExecutor CreateExecutor(Database database, MongoConfiguration configuration, ChangeTracker changeTracker)
        {
            return new MongoQueryExecutor(database, configuration, changeTracker);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoQueryable&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="mongo">The mongo.</param>
        public MongoQueryable(Database database, MongoConfiguration configuration, ChangeTracker changeTracker)
            : base(CreateExecutor(database, configuration, changeTracker))
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