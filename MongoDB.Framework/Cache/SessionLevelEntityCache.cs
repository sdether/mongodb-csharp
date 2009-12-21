using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Cache
{
    public sealed class SessionLevelEntityCache : IEntityCache
    {
        #region Private Fields

        private Dictionary<string, object> cache;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionLevelEntityCache"/> class.
        /// </summary>
        public SessionLevelEntityCache()
        {
            this.cache = new Dictionary<string, object>();
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="SessionLevelEntityCache"/> is reclaimed by garbage collection.
        /// </summary>
        ~SessionLevelEntityCache()
        {
            this.Dispose(false);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.EnsureNotDisposed();
            this.cache.Clear();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Remove(object entity)
        {
            this.EnsureNotDisposed();
            string idToRemove = null;

            foreach (var pair in this.cache)
            {
                if (pair.Value == entity)
                    idToRemove = pair.Key;
                    break;
            }

            if (idToRemove != null)
            {
                this.cache.Remove(idToRemove);
            }
        }

        /// <summary>
        /// Removes all instances of.
        /// </summary>
        /// <param name="type">The type.</param>
        public void RemoveAllInstancesOf(Type type)
        {
            this.EnsureNotDisposed();
            List<string> idsToRemove = new List<string>();
            foreach (var pair in this.cache)
            {
                if (type.IsAssignableFrom(pair.Value.GetType()))
                    idsToRemove.Add(pair.Key);
            }

            foreach (var id in idsToRemove)
                this.cache.Remove(id);
        }

        /// <summary>
        /// Stores the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="id">The id.</param>
        /// <param name="entity">The entity.</param>
        public void Store(string id, object entity)
        {
            this.EnsureNotDisposed();
            this.cache[id] = entity;
        }

        /// <summary>
        /// Tries to find.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="id">The id.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public bool TryToFind(string id, out object entity)
        {
            this.EnsureNotDisposed();
            if (!this.cache.TryGetValue(id, out entity)) 
                return false;

            return true;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            if (this.cache != null)
            {
                this.cache.Clear();
                this.cache = null;
            }
        }

        private void EnsureNotDisposed()
        {
            if (this.cache == null)
                throw new ObjectDisposedException("SessionLevelEntityCache");
        }

        #endregion
    }
}