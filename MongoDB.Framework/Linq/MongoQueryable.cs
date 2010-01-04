using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Remotion.Data.Linq;

using MongoDB.Driver;
using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Tracking;

namespace MongoDB.Framework.Linq
{
    public class MongoQueryable<TEntity> : QueryableBase<TEntity>
    {
        /// <summary>
        /// Creates the executor.
        /// </summary>
        /// <param name="mongoContext">The mongo context.</param>
        /// <param name="mongoContextCache">The mongo context cache.</param>
        /// <param name="changeTracker">The change tracker.</param>
        /// <returns></returns>
        private static IQueryExecutor CreateExecutor(IMongoContextImplementor mongoContext, IMongoContextCache mongoContextCache, IChangeTracker changeTracker)
        {
            return new MongoQueryExecutor(mongoContext, mongoContextCache, changeTracker);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoQueryable&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="mongoContext">The mongo context.</param>
        /// <param name="mongoContextCache">The mongo context cache.</param>
        /// <param name="changeTracker">The change tracker.</param>
        public MongoQueryable(IMongoContextImplementor mongoContext, IMongoContextCache mongoContextCache, IChangeTracker changeTracker)
            : base(CreateExecutor(mongoContext, mongoContextCache, changeTracker))
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