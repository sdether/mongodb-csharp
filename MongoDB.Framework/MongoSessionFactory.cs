using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Tracking;
using MongoDB.Framework.Proxy;

namespace MongoDB.Framework
{
    public class MongoSessionFactory : IMongoSessionFactory
    {
        #region Private Fields

        private string databaseName;
        private bool initialized;
        private object initializationObject;
        private IMappingStore mappingStore;
        private IMongoFactory mongoFactory;
        private IProxyGenerator proxyGenerator;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the mapping store.
        /// </summary>
        /// <value>The mapping store.</value>
        public IMappingStore MappingStore
        {
            get { return this.mappingStore; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoSessionFactory"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="mongo">The mongo.</param>
        public MongoSessionFactory(string databaseName, IMappingStore mappingStore, IMongoFactory mongoFactory, IProxyGenerator proxyGenerator)
        {
            if (string.IsNullOrEmpty(databaseName))
                throw new ArgumentException("Cannot be null or empty.", "databaseName");
            if (mappingStore == null)
                throw new ArgumentNullException("mappingStore");
            if (mongoFactory == null)
                throw new ArgumentNullException("mongoFactory");
            if (proxyGenerator == null)
                throw new ArgumentNullException("proxyGenerator");

            this.databaseName = databaseName;
            this.initialized = false;
            this.initializationObject = new object();
            this.mappingStore = mappingStore;
            this.mongoFactory = mongoFactory;
            this.proxyGenerator = proxyGenerator;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Opens the mongo session.
        /// </summary>
        /// <returns></returns>
        public IMongoSession OpenMongoSession()
        {
            var mongo = this.mongoFactory.CreateMongo();
            mongo.Connect();

            var database = mongo.getDB(this.databaseName);

            if(!this.initialized)
            {
                lock(this.initializationObject)
                {
                    if(!this.initialized)
                    {
                        this.Initialize(database);
                        this.initialized = true;
                    }
                }
            }

            return new MongoSession(
                this.mappingStore,
                this.proxyGenerator,
                mongo, 
                database);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Initializes the specified database.
        /// </summary>
        /// <param name="database">The database.</param>
        protected virtual void Initialize(Database database)
        {
            this.CreateIndexes(database);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates the indexes.
        /// </summary>
        private void CreateIndexes(Database database)
        {
            foreach (var classMap in this.mappingStore.ClassMaps)
            {
                //getting a collection is more expensive than counting indexes, so let's make this as fast as possible...
                if (classMap.Indexes.Count() == 0)
                    continue;

                var collectionMetaData = database.GetCollection(classMap.CollectionName).MetaData;
                foreach (var index in classMap.Indexes)
                {
                    Document fieldsAndDirections = new Document();
                    foreach (var part in index.Parts)
                    {
                        fieldsAndDirections.Add(
                            part.Key,
                            part.Value == IndexDirection.Ascending ? 1 : -1);
                    }

                    collectionMetaData.CreateIndex(index.Name, fieldsAndDirections, index.IsUnique);
                }
            }
        }

        #endregion
    }
}