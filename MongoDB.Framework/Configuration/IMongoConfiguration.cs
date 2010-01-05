using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.Mapping;
using MongoDB.Framework.Proxy;

namespace MongoDB.Framework.Configuration
{
    public interface IMongoConfiguration
    {
        /// <summary>
        /// Gets the name of the database.
        /// </summary>
        /// <value>The name of the database.</value>
        string DatabaseName { get; }

        /// <summary>
        /// Gets the mapping store.
        /// </summary>
        /// <value>The mapping store.</value>
        IMappingStore MappingStore { get; }

        /// <summary>
        /// Gets the mongo factory.
        /// </summary>
        /// <value>The mongo factory.</value>
        IMongoFactory MongoFactory { get; }

        /// <summary>
        /// Gets the proxy generator.
        /// </summary>
        /// <value>The proxy generator.</value>
        IProxyGenerator ProxyGenerator { get; }

        /// <summary>
        /// Creates the context factory.
        /// </summary>
        /// <returns></returns>
        IMongoContextFactory CreateContextFactory();
    }
}