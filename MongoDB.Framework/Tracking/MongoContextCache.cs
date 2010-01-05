using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Tracking
{
    public class MongoSessionCache : IMongoSessionCache
    {
        private readonly Dictionary<string, Dictionary<object, object>> cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoSessionCache"/> class.
        /// </summary>
        public MongoSessionCache()
        {
            cache = new Dictionary<string, Dictionary<object, object>>();
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public void Clear()
        {
            this.cache.Clear();
        }

        /// <summary>
        /// Removes the specified collection name.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="entity">The entity.</param>
        public void Remove(string collectionName, object entity)
        {
            Dictionary<object, object> idCache;
            if (!cache.TryGetValue(collectionName, out idCache))
                return;

            object keyToRemove = null;

            foreach (var pair in idCache)
            {
                if (pair.Value == entity)
                    keyToRemove = pair.Key;
            }

            if (keyToRemove != null)
            {
                idCache.Remove(keyToRemove);
            }
        }

        /// <summary>
        /// Stores the specified collection name.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="id">The id.</param>
        /// <param name="entity">The entity.</param>
        public void Store(string collectionName, object id, object entity)
        {
            if (string.IsNullOrEmpty(collectionName))
	            throw new ArgumentException("Cannot be null or empty.", "collectionName");
            if (id == null)
                throw new ArgumentNullException("id");
            if (entity == null)
                throw new ArgumentNullException("entity");

            Dictionary<object, object> idCache;
            if (!cache.TryGetValue(collectionName, out idCache))
                cache[collectionName] = idCache = new Dictionary<object, object>();

            idCache[id] = entity;
        }

        /// <summary>
        /// Tries to find.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public object TryToFind(string collectionName, object id)
        {
            Dictionary<object, object> idCache;
            if (!cache.TryGetValue(collectionName, out idCache))
                return null;

            object entity;
            if (!idCache.TryGetValue(id, out entity))
                return null;

            return entity;
        }
    }
}