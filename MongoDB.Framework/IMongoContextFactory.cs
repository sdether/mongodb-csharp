using System;

using MongoDB.Framework.Configuration;

namespace MongoDB.Framework
{
    public interface IMongoContextFactory
    {
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        IMongoConfiguration Configuration { get; }

        /// <summary>
        /// Creates the context.
        /// </summary>
        /// <returns></returns>
        MongoContext CreateContext();
    }
}
