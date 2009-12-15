using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Remotion.Data.Linq;
using MongoDB.Driver;

using MongoDB.Framework.Linq.Visitors;
using System.Diagnostics;

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
            var spec = MongoQueryModelVisitor.TranslateToMongoQuerySpec(queryModel, this.entityMapper);

            var collection = this.GetCollectionFromType(typeof(T));

            var cursor = collection.Find(spec.Query, spec.Limit, spec.Skip, spec.Fields);

            return MapFromDocuments<T>(cursor.Documents);
        }

        public T ExecuteScalar<T>(QueryModel queryModel)
        {
            throw new NotImplementedException();
        }

        public T ExecuteSingle<T>(QueryModel queryModel, bool returnDefaultWhenEmpty)
        {
            throw new NotImplementedException();
        }

        private IMongoCollection GetCollectionFromType(Type type)
        {
            var rootEntityMap = this.entityMapper.Configuration.GetRootEntityMapFor(type);
            return this.database.GetCollection(rootEntityMap.CollectionName);
        }

        private IEnumerable<T> MapFromDocuments<T>(IEnumerable<Document> documents)
        {
            foreach (var document in documents)
            {
                Console.WriteLine(document.ToString());
                yield return (T)this.entityMapper.MapDocumentToEntity(document, typeof(T));
            }
        }
    }
}