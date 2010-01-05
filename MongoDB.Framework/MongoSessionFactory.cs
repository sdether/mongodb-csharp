using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Tracking;

namespace MongoDB.Framework
{
    public class MongoSessionFactory : IMongoSessionFactory
    {
        #region Private Fields

        private bool initialized;
        private object initializeObject = new object();

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public IMongoConfiguration Configuration { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoSessionFactory"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="mongo">The mongo.</param>
        public MongoSessionFactory(IMongoConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            this.Configuration = configuration;
            this.initialized = false;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Opens the mongo session.
        /// </summary>
        /// <returns></returns>
        public IMongoSession OpenMongoSession()
        {
            var mongo = this.Configuration.MongoFactory.CreateMongo();
            mongo.Connect();

            var database = mongo.getDB(this.Configuration.DatabaseName);

            if(!this.initialized)
            {
                lock(this.initializeObject)
                {
                    if(!this.initialized)
                    {
                        this.Initialize(database);
                        this.initialized = true;
                    }
                }
            }

            return new MongoSession(
                new IndexingMappingStore(this.Configuration.MappingStore, database), 
                this.Configuration.ProxyGenerator,
                mongo, 
                database);
        }

        #endregion

        #region Protected Methods

         ///<summary>
         ///Initializes the specified database.
         ///</summary>
         ///<param name="database">The database.</param>
        protected virtual void Initialize(Database database)
        {
        }

        #endregion
    }
}