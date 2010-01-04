using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Configuration;
using MongoDB.Driver;
using MongoDB.Framework.Tracking;
using MongoDB.Framework.Configuration.Mapping;

namespace MongoDB.Framework
{
    public class MongoContextFactory : IMongoContextFactory
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
        /// Initializes a new instance of the <see cref="MongoContextFactory"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="mongo">The mongo.</param>
        public MongoContextFactory(IMongoConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            this.Configuration = configuration;
            this.initialized = false;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the context.
        /// </summary>
        /// <returns></returns>
        public MongoContext CreateContext()
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
            return new MongoContext(
                new IndexingMappingStore(this.Configuration.MappingStore, database), 
                new MongoContextCache(), 
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