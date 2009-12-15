using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using MongoDB.Driver;
using Remotion.Data.Linq;

namespace MongoDB.Framework.Linq
{
    public class MongoQueryable<TEntity> : QueryableBase<TEntity>
    {
        /// <summary>
        /// Creates the executor.
        /// </summary>
        /// <param name="mongo">The mongo.</param>
        /// <returns></returns>
        private static IQueryExecutor CreateExecutor(Database database, EntityMapper entityMapper)
        {
            return new MongoQueryExecutor(database, entityMapper);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoQueryable&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="mongo">The mongo.</param>
        public MongoQueryable(Database database, EntityMapper entityMapper)
            : base(CreateExecutor(database, entityMapper))
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