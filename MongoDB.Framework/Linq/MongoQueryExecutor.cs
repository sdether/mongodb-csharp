using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Remotion.Data.Linq;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;
using MongoDB.Framework.Linq.Visitors;
using MongoDB.Framework.Tracking;
using MongoDB.Framework.Configuration.Visitors;
using Remotion.Data.Linq.Clauses.ResultOperators;

namespace MongoDB.Framework.Linq
{
    public class MongoQueryExecutor : IQueryExecutor
    {
        private ChangeTracker changeTracker;
        private Database database;
        private EntityMapper entityMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoQueryExecutor"/> class.
        /// </summary>
        /// <param name="mongo">The mongo.</param>
        public MongoQueryExecutor(Database database, EntityMapper entityMapper, ChangeTracker changeTracker)
        {
            if (database == null)
                throw new ArgumentNullException("database");
            if (entityMapper == null)
                throw new ArgumentNullException("entityMapper");
            if (changeTracker == null)
                throw new ArgumentNullException("changeTracker");

            this.changeTracker = changeTracker;
            this.database = database;
            this.entityMapper = entityMapper;
        }

        public IEnumerable<T> ExecuteCollection<T>(QueryModel queryModel)
        {
            var spec = CollectionQueryModelVisitor<T>.CreateMongoQuerySpecification(queryModel, this.entityMapper);

            var itemType = queryModel.MainFromClause.ItemType;
            var rootEntityMap = this.entityMapper.Configuration.GetRootEntityMapFor(itemType);
            this.AddDiscriminatingKeyIfNecessary(itemType, rootEntityMap, spec.Query);
            var collection = this.database.GetCollection(rootEntityMap.CollectionName);
            IEnumerable<Document> documents;
            if (spec.IsFindOne)
            {
                var document = collection.FindOne(spec.Query);
                if (document == null)
                    documents = Enumerable.Empty<Document>();
                else
                    documents = new[] { document };
            }
            else
            {
                var cursor = collection.Find(spec.GetCompleteQuery(), spec.Limit, spec.Skip, spec.Projection.Fields);
                documents = cursor.Documents;
            }

            foreach(var document in documents)
            {
                var entity = spec.Projection.Projector(this.entityMapper.Configuration, document);
                if (typeof(T) == itemType)
                    this.changeTracker.Track(document, entity);
                yield return entity;
            }
        }

        public T ExecuteScalar<T>(QueryModel queryModel)
        {
            var scalarVisitor = new ScalarQueryModelVisitor(this.entityMapper.Configuration);
            scalarVisitor.VisitQueryModel(queryModel);

            var entityType = queryModel.MainFromClause.ItemType;
            var rootEntityMap = this.entityMapper.Configuration.GetRootEntityMapFor(entityType);
            this.AddDiscriminatingKeyIfNecessary(entityType, rootEntityMap, scalarVisitor.Query);

            var collection = this.database.GetCollection(rootEntityMap.CollectionName);

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

        private void AddDiscriminatingKeyIfNecessary(Type entityType, RootEntityMap rootEntityMap, Document query)
        {
            if (rootEntityMap.Type != entityType)
            {
                var discriminatedEntityMap = rootEntityMap.GetDiscriminatedEntityMapByType(entityType);
                query[rootEntityMap.DiscriminatingDocumentKey] = discriminatedEntityMap.DiscriminatingValue;
            }
        }
    }
}