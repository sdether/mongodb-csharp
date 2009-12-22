using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Remotion.Data.Linq;
using Remotion.Data.Linq.Clauses.ResultOperators;

using MongoDB.Driver;
using MongoDB.Framework.Hydration;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Tracking;
using MongoDB.Framework.Linq.Visitors;

namespace MongoDB.Framework.Linq
{
    public class MongoQueryExecutor : IQueryExecutor
    {
        private ChangeTracker changeTracker;
        private Database database;
        private IEntityHydrator hydrator;
        private MappingStore mappingStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoQueryExecutor"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="changeTracker">The change tracker.</param>
        /// <param name="hydrator">The hydrator.</param>
        /// <param name="database">The database.</param>
        public MongoQueryExecutor(MappingStore mappingStore, ChangeTracker changeTracker, IEntityHydrator hydrator, Database database)
        {
            if (mappingStore == null)
                throw new ArgumentNullException("mappingStore");
            if (changeTracker == null)
                throw new ArgumentNullException("changeTracker");
            if (hydrator == null)
                throw new ArgumentNullException("hydrator");
            if (database == null)
                throw new ArgumentNullException("database");

            this.changeTracker = changeTracker;
            this.database = database;
            this.hydrator = hydrator;
            this.mappingStore = mappingStore;
        }

        public IEnumerable<T> ExecuteCollection<T>(QueryModel queryModel)
        {
            var spec = CollectionQueryModelVisitor<T>.CreateMongoQuerySpecification(this.mappingStore, queryModel);

            throw new NotImplementedException();
            //var itemType = queryModel.MainFromClause.ItemType;
            //var rootEntityMap = this.configuration.GetRootEntityMapFor(itemType);
            //this.AddDiscriminatingKeyIfNecessary(itemType, rootEntityMap, spec.Query);
            //var collection = this.database.GetCollection(rootEntityMap.CollectionName);
            //IEnumerable<Document> documents;
            //if (spec.IsFindOne)
            //{
            //    var document = collection.FindOne(spec.Query);
            //    if (document == null)
            //        documents = Enumerable.Empty<Document>();
            //    else
            //        documents = new[] { document };
            //}
            //else
            //{
            //    var cursor = collection.Find(spec.GetCompleteQuery(), spec.Limit, spec.Skip, spec.Projection.Fields);
            //    documents = cursor.Documents;
            //}

            //foreach(var document in documents)
            //{
            //    var entity = spec.Projection.Projector(this.configuration, document);
            //    if (typeof(T) == itemType)
            //        this.changeTracker.Track(document, entity);
            //    yield return entity;
            //}
        }

        public T ExecuteScalar<T>(QueryModel queryModel)
        {
            //var scalarVisitor = new ScalarQueryModelVisitor(this.configuration);
            //scalarVisitor.VisitQueryModel(queryModel);

            //var entityType = queryModel.MainFromClause.ItemType;
            //var rootEntityMap = this.configuration.GetRootEntityMapFor(entityType);
            //this.AddDiscriminatingKeyIfNecessary(entityType, rootEntityMap, scalarVisitor.Query);

            //var collection = this.database.GetCollection(rootEntityMap.CollectionName);

            //if (scalarVisitor.IsCount)
            //{
            //    return (T)Convert.ChangeType(collection.Count(scalarVisitor.Query), typeof(T));
            //}

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