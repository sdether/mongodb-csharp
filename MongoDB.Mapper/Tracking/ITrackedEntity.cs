using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Mapper.Tracking
{
    public interface ITrackedEntity
    {
        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <value>The current.</value>
        object Current { get; }

        /// <summary>
        /// Gets the original.
        /// </summary>
        /// <value>The original.</value>
        Document Original { get; }

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>The state.</value>
        TrackedEntityState State { get; }

        /// <summary>
        /// Moves to modified.
        /// </summary>
        void MoveToModified();

        /// <summary>
        /// Moves to possible modified.
        /// </summary>
        /// <param name="original">The original.</param>
        void MoveToPossibleModified(Document original);

        /// <summary>
        /// Moves to inserted.
        /// </summary>
        void MoveToInserted();

        /// <summary>
        /// Moves to deleted.
        /// </summary>
        void MoveToDeleted();
    }
}