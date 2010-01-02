using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping;
using MongoDB.Driver;

namespace MongoDB.Framework.Configuration
{
    public class MongoConfiguration : IMongoConfiguration
    {
        #region Public Properties

        /// <summary>
        /// Gets the name of the database.
        /// </summary>
        /// <value>The name of the database.</value>
        public string DatabaseName { get; private set; }

        /// <summary>
        /// Gets the mapping store.
        /// </summary>
        /// <value>The mapping store.</value>
        public IMappingStore MappingStore { get; private set; }

        /// <summary>
        /// Gets the mongo factory.
        /// </summary>
        /// <value>The mongo factory.</value>
        public IMongoFactory MongoFactory { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoConfiguration"/> class.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="mappingStore">The mapping store.</param>
        public MongoConfiguration(string databaseName, IMappingStore mappingStore)
        {
            if (string.IsNullOrEmpty(databaseName))
                throw new ArgumentException("Cannot be null or empty.", "databaseName");
            if (mappingStore == null)
                throw new ArgumentNullException("mappingStore");

            this.DatabaseName = databaseName;
            this.MappingStore = mappingStore;
            this.MongoFactory = new DefaultMongoFactory();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the context factory.
        /// </summary>
        /// <returns></returns>
        public MongoContextFactory CreateContextFactory()
        {
            return new MongoContextFactory(this);
        }

        #endregion
    }
}