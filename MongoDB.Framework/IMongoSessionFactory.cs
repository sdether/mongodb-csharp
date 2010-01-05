using System;

using MongoDB.Framework.Configuration;

namespace MongoDB.Framework
{
    public interface IMongoSessionFactory
    {
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        IMongoConfiguration Configuration { get; }

        /// <summary>
        /// Opens the mongo session.
        /// </summary>
        /// <returns></returns>
        IMongoSession OpenMongoSession();
    }
}
