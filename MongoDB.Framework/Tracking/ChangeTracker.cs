using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Tracking
{
    public abstract class ChangeTracker : IDisposable
    {
        /// <summary>
        /// Gets the change set.
        /// </summary>
        /// <returns></returns>
        public abstract ChangeSet GetChangeSet();

        /// <summary>
        /// Gets the tracked object and begins tracking it if not already.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public abstract TrackedObject GetTrackedObject(object obj);

        /// <summary>
        /// Tracks the specified object.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <param name="current">The current.</param>
        public abstract TrackedObject Track(Document original, object current);

        /// <summary>
        /// Tries to get a tracked object by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public abstract bool TryGetTrackedObjectById(object id, out TrackedObject entity);

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="ChangeTracker"/> is reclaimed by garbage collection.
        /// </summary>
        ~ChangeTracker()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        { }

        public abstract void Initialize(IMongoContext mongoContext);
    }
}