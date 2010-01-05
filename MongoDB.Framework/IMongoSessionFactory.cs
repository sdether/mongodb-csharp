using System;

using MongoDB.Framework.Configuration;
using MongoDB.Framework.Mapping;

namespace MongoDB.Framework
{
    public interface IMongoSessionFactory
    {
        /// <summary>
        /// Gets the mapping store.
        /// </summary>
        /// <value>The mapping store.</value>
        IMappingStore MappingStore { get; }

        /// <summary>
        /// Opens the mongo session.
        /// </summary>
        /// <returns></returns>
        IMongoSession OpenMongoSession();
    }
}
