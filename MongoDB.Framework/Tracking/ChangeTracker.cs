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
    }
}