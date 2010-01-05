using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Proxy;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Proxy.Castle;

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
        public IMapModelRegistry MapModelRegistry { get; private set; }

        /// <summary>
        /// Gets the mongo factory.
        /// </summary>
        /// <value>The mongo factory.</value>
        public IMongoFactory MongoFactory { get; set; }

        /// <summary>
        /// Gets the proxy generator.
        /// </summary>
        /// <value>The proxy generator.</value>
        public IProxyGenerator ProxyGenerator { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoConfiguration"/> class.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="mappingStore">The mapping store.</param>
        public MongoConfiguration(string databaseName, IMapModelRegistry mapModelRegistry)
        {
            if (string.IsNullOrEmpty(databaseName))
                throw new ArgumentException("Cannot be null or empty.", "databaseName");
            if (mapModelRegistry == null)
                throw new ArgumentNullException("mapModelRegistry");

            this.DatabaseName = databaseName;
            this.MapModelRegistry = mapModelRegistry;
            this.MongoFactory = new DefaultMongoFactory();
            this.ProxyGenerator = new CastleProxyGenerator();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the mongo session factory.
        /// </summary>
        /// <returns></returns>
        public virtual IMongoSessionFactory CreateMongoSessionFactory()
        {
            return new MongoSessionFactory(
                this.DatabaseName,
                this.MapModelRegistry.BuildMappingStore(),
                this.MongoFactory,
                this.ProxyGenerator);
        }

        #endregion
    }
}