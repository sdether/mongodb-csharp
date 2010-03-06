using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Mapper.Configuration;
using MongoDB.Mapper.Mapping;
using MongoDB.Mapper.Proxy;
using MongoDB.Mapper.Tracking;
using MongoDB.Driver;

namespace MongoDB.Mapper
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

        /// <summary>
        /// Maps to document.
        /// </summary>
        /// <param name="classMap">The class map.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Document MapToDocument(ClassMapBase classMap, object entity);

        /// <summary>
        /// Maps to entity.
        /// </summary>
        /// <param name="classMap">The class map.</param>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        object MapToEntity(ClassMapBase classMap, Document document);
    }
}