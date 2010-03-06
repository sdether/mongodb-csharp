using System;

using MongoDB.Mapper.Configuration;
using MongoDB.Mapper.Mapping;

namespace MongoDB.Mapper
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
