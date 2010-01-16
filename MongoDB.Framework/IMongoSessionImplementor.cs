using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.Configuration;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Proxy;
using MongoDB.Framework.Tracking;

namespace MongoDB.Framework
{
    public interface IMongoSessionImplementor : IMongoSession
    {
        /// <summary>
        /// Gets the mapping store.
        /// </summary>
        /// <value>The mapping store.</value>
        IMappingStore MappingStore { get; }

        /// <summary>
        /// Gets the proxy generator.
        /// </summary>
        /// <value>The proxy generator.</value>
        IProxyGenerator ProxyGenerator { get; }

        /// <summary>
        /// Gets the session cache.
        /// </summary>
        /// <value>The session cache.</value>
        IMongoSessionCache SessionCache { get; }
    }
}