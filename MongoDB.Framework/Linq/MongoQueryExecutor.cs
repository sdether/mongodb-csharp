using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Remotion.Data.Linq;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;
using MongoDB.Framework.Linq.Visitors;
using MongoDB.Framework.Tracking;

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
            var spec = MongoQueryModelVisitor.CreateMongoQuerySpecification(queryModel, this.entityMapper.Configuration);

            var rootEntityMap = this.entityMapper.Configuration.GetRootEntityMapFor(queryModel.MainFromClause.ItemType);
            this.AddDiscriminatingKeyIfNecessary(queryModel.MainFromClause.ItemType, rootEntityMap, spec);

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
                var cursor = collection.Find(spec.GetCompleteQuery(), spec.Limit, spec.Skip, spec.Fields);
                documents = cursor.Documents;
            }

            foreach (var document in documents)
            {
                var entity = (T)this.entityMapper.MapDocumentToEntity(document, typeof(T));
                this.changeTracker.Track(document, entity);
                yield return entity;
            }
        }

        public T ExecuteScalar<T>(QueryModel queryModel)
        {
            Type entityType = queryModel.MainFromClause.ItemType;
            var spec = MongoQueryModelVisitor.CreateMongoQuerySpecification(queryModel, this.entityMapper.Configuration);
            var rootEntityMap = this.entityMapper.Configuration.GetRootEntityMapFor(entityType);
            this.AddDiscriminatingKeyIfNecessary(entityType, rootEntityMap, spec);

            var collection = this.database.GetCollection(rootEntityMap.CollectionName);

            if (spec.IsCount)
            {
                return (T)Convert.ChangeType(collection.Count(spec.Query), typeof(T));
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

        private void AddDiscriminatingKeyIfNecessary(Type entityType, RootEntityMap rootEntityMap, MongoQuerySpecification spec)
        {
            if (rootEntityMap.Type != entityType)
            {
                var discriminatedEntityMap = rootEntityMap.GetDiscriminatedEntityMapByType(entityType);
                spec.Query[rootEntityMap.DiscriminatingDocumentKey] = discriminatedEntityMap.DiscriminatingValue;
            }
        }
    }
}