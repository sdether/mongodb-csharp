using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Remotion.Data.Linq;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;
using MongoDB.Framework.Linq.Visitors;

namespace MongoDB.Framework.Linq
{
    public class MongoQueryExecutor : IQueryExecutor
    {
        private Database database;
        private EntityMapper entityMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoQueryExecutor"/> class.
        /// </summary>
        /// <param name="mongo">The mongo.</param>
        public MongoQueryExecutor(Database database, EntityMapper entityMapper)
        {
            if (database == null)
                throw new ArgumentNullException("database");
            if (entityMapper == null)
                throw new ArgumentNullException("entityMapper");

            this.database = database;
            this.entityMapper = entityMapper;
        }

        public IEnumerable<T> ExecuteCollection<T>(QueryModel queryModel)
        {
            var spec = MongoQueryModelVisitor.CreateMongoQuerySpecification(queryModel, this.entityMapper.Configuration);
            
            var rootEntityMap = this.entityMapper.Configuration.GetRootEntityMapFor(typeof(T));
            this.AddDiscriminatingKeyIfNecessary(typeof(T), rootEntityMap, spec);

            var collection = this.database.GetCollection(rootEntityMap.CollectionName);
            var cursor = collection.Find(spec.Query, spec.Limit, spec.Skip, spec.Projection);

            return MapFromDocuments<T>(cursor.Documents);
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

            throw new NotSupportedException();
        }

        public T ExecuteSingle<T>(QueryModel queryModel, bool returnDefaultWhenEmpty)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<T> MapFromDocuments<T>(IEnumerable<Document> documents)
        {
            foreach (var document in documents)
            {
                yield return (T)this.entityMapper.MapDocumentToEntity(document, typeof(T));
            }
        }

        private void AddDiscriminatingKeyIfNecessary(Type entityType, RootEntityMap rootEntityMap, MongoQuerySpecification spec)
        {
            if (rootEntityMap.Type != entityType)
            {
                var discriminatedEntityMap = rootEntityMap.GetDiscriminatedEntityMapByType(entityType);
                spec.Query[rootEntityMap.DiscriminateDocumentKey] = discriminatedEntityMap.DiscriminatingValue;
            }
        }
    }
}