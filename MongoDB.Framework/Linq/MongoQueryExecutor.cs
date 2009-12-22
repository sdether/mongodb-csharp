using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Remotion.Data.Linq;
using Remotion.Data.Linq.Clauses.ResultOperators;

using MongoDB.Driver;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Tracking;
using MongoDB.Framework.Linq.Visitors;
using System.Collections;
using MongoDB.Framework.Persistence;

namespace MongoDB.Framework.Linq
{
    public class MongoQueryExecutor : IQueryExecutor
    {
        private ChangeTracker changeTracker;
        private Database database;
        private DocumentToEntityTranslator documentToEntityTranslator;
        private MappingStore mappingStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoQueryExecutor"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="changeTracker">The change tracker.</param>
        /// <param name="hydrator">The hydrator.</param>
        /// <param name="database">The database.</param>
        public MongoQueryExecutor(MappingStore mappingStore, ChangeTracker changeTracker, Database database)
        {
            if (mappingStore == null)
                throw new ArgumentNullException("mappingStore");
            if (changeTracker == null)
                throw new ArgumentNullException("changeTracker");
            if (database == null)
                throw new ArgumentNullException("database");

            this.changeTracker = changeTracker;
            this.database = database;
            this.mappingStore = mappingStore;
        }

        public IEnumerable<T> ExecuteCollection<T>(QueryModel queryModel)
        {
            var spec = CollectionQueryModelVisitor.CreateMongoQuerySpecification(this.mappingStore, queryModel);
            var documentMap = this.mappingStore.GetDocumentMapFor(queryModel.MainFromClause.ItemType);
            var collection = this.database.GetCollection(documentMap.CollectionName);
            var findAction = new FindAction(this.mappingStore, this.changeTracker, collection);
            foreach (var entity in findAction.Find(documentMap.EntityType, spec.Conditions, spec.Limit, spec.Skip, spec.OrderBy, spec.Projection.Fields))
                yield return (T)spec.Projection.Projector(new ResultObjectMapping() { { queryModel.MainFromClause, entity } });
        }

        public T ExecuteScalar<T>(QueryModel queryModel)
        {
            var scalarVisitor = new ScalarQueryModelVisitor(this.mappingStore);
            scalarVisitor.VisitQueryModel(queryModel);

            var itemType = queryModel.MainFromClause.ItemType;
            var documentMap = this.mappingStore.GetDocumentMapFor(itemType);
            if (documentMap.IsPolymorphic && documentMap.Discriminator != null)
                scalarVisitor.Query[documentMap.DiscriminatorKey] = documentMap.Discriminator;

            var collection = this.database.GetCollection(documentMap.CollectionName);

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