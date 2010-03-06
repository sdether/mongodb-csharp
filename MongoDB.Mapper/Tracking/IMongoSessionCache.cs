using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper.Tracking
{
    public interface IMongoSessionCache
    {
        /// <summary>
        /// Clears the cache.
        /// </summary>
        void Clear();

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="entity">The entity.</param>
        void Remove(string collectionName, object entity);

        /// <summary>
        /// Stores the entity.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="id">The id.</param>
        /// <param name="entity">The entity.</param>
        void Store(string collectionName, object id, object entity);

        /// <summary>
        /// Tries to find and entity in the specified collection with the specified id.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="id">The id.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        bool TryToFind(string collectionName, object id, out object entity);
    }
}