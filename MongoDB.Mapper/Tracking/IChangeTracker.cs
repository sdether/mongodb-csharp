using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper.Tracking
{
    public interface IChangeTracker : IDisposable
    {
        /// <summary>
        /// Gets the change set.
        /// </summary>
        /// <returns></returns>
        ChangeSet GetChangeSet();

        /// <summary>
        /// Gets the tracked entity, or creates one if it does not exist.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        ITrackedEntity GetTrackedEntity(object entity);

        /// <summary>
        /// Stops the tracking.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void StopTracking(object entity);
    }
}