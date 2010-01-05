using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Remotion.Data.Linq;
using Remotion.Data.Linq.Clauses.ResultOperators;

using MongoDB.Driver;
using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Tracking;
using MongoDB.Framework.Linq.Visitors;
using System.Collections;
using MongoDB.Framework.Persistence;

namespace MongoDB.Framework.Linq
{
    public class MongoQueryExecutor : IQueryExecutor
    {
        private IChangeTracker changeTracker;
        private IMongoSessionCache mongoSessionCache;
        private IMongoSessionImplementor mongoSession;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoQueryExecutor"/> class.
        /// </summary>
        /// <param name="mongoSession">The mongo session.</param>
        /// <param name="mongoSessionCache">The mongo session cache.</param>
        /// <param name="changeTracker">The change tracker.</param>
        public MongoQueryExecutor(IMongoSessionImplementor mongoSession, IMongoSessionCache mongoSessionCache, IChangeTracker changeTracker)
        {
            if (mongoSession == null)
                throw new ArgumentNullException("mongoSession");
            if (mongoSessionCache == null)
                throw new ArgumentNullException("mongoSessionCache");
            if (changeTracker == null)
                throw new ArgumentNullException("changeTracker");

            this.changeTracker = changeTracker;
            this.mongoSession = mongoSession;
            this.mongoSessionCache = mongoSessionCache;
        }

        public IEnumerable<T> ExecuteCollection<T>(QueryModel queryModel)
        {
            var spec = CollectionQueryModelVisitor.CreateMongoQuerySpecification(this.mongoSession, queryModel);
            var classMap = this.mongoSession.MappingStore.GetClassMapFor(queryModel.MainFromClause.ItemType);
            var findAction = new FindAction(this.mongoSession, this.mongoSessionCache, changeTracker);
            foreach (var entity in findAction.Find(classMap.Type, spec.Conditions, spec.Limit, spec.Skip, spec.OrderBy, spec.Projection.Fields))
                yield return (T)spec.Projection.Projector(new ResultObjectMapping() { { queryModel.MainFromClause, entity } });
        }

        public T ExecuteScalar<T>(QueryModel queryModel)
        {
            var scalarVisitor = new ScalarQueryModelVisitor(this.mongoSession);
            scalarVisitor.VisitQueryModel(queryModel);

            var itemType = queryModel.MainFromClause.ItemType;
            var classMap = this.mongoSession.MappingStore.GetClassMapFor(itemType);
            if (classMap.IsPolymorphic && classMap.Discriminator != null)
                scalarVisitor.Query[classMap.DiscriminatorKey] = classMap.Discriminator;

            //TODO: Convert to PersistenceAction
            var collection = this.mongoSession.Database.GetCollection(classMap.CollectionName);

            if (scalarVisitor.IsCount)
            {
                return (T)Convert.ChangeType(collection.Count(scalarVisitor.Query), typeof(T));
            }

            throw new NotSupportedException("Count is the only allowed aggregate operation.");
        }

        public T ExecuteSingle<T>(QueryModel queryModel, bool returnDefaultWhenEmpty)
        {
            var items = this.ExecuteCollection<T>(queryModel).ToList();
            if (items.Count() == 0 && returnDefaultWhenEmpty)
                return default(T);

            return items.First();
        }
    }
}