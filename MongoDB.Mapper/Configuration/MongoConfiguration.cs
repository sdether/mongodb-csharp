using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Mapper.Proxy;
using MongoDB.Mapper.Mapping;
using MongoDB.Mapper.Configuration.Mapping;
using MongoDB.Mapper.Proxy.Castle;
using MongoDB.Mapper.Mapping.Auto;

namespace MongoDB.Mapper.Configuration
{
    public class MongoConfiguration
    {
        /// <summary>
        /// Gets the name of the database.
        /// </summary>
        /// <value>The name of the database.</value>
        public string DatabaseName { get; private set; }

        /// <summary>
        /// Gets or sets the mapping store.
        /// </summary>
        /// <value>The mapping store.</value>
        public IMappingStore MappingStore { get; set; }

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

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoConfiguration"/> class.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        public MongoConfiguration(string databaseName)
        {
            if (string.IsNullOrEmpty(databaseName))
                throw new ArgumentException("Cannot be null or empty.", "databaseName");

            this.DatabaseName = databaseName;
            this.MappingStore = new AutoMappingStore();
            this.MongoFactory = new DefaultMongoFactory();
            this.ProxyGenerator = new CastleProxyGenerator();
        }

        /// <summary>
        /// Creates the mongo session factory.
        /// </summary>
        /// <returns></returns>
        public virtual IMongoSessionFactory CreateMongoSessionFactory()
        {
            return new MongoSessionFactory(
                this.DatabaseName,
                this.MappingStore,
                this.MongoFactory,
                this.ProxyGenerator);
        }
    }
}