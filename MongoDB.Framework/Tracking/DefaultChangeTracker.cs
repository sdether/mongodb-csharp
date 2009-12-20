﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;

namespace MongoDB.Framework.Tracking
{
    public class DefaultChangeTracker : ChangeTracker
    {
        #region Private Fields

        private MongoConfiguration configuration;
        private Dictionary<object, TrackedObject> trackedObjects;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultChangeTracker"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public DefaultChangeTracker(MongoConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            this.configuration = configuration;
            this.trackedObjects = new Dictionary<object, TrackedObject>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the change set.
        /// </summary>
        /// <returns></returns>
        public override ChangeSet GetChangeSet()
        {
            List<object> added = new List<object>();
            List<object> modified = new List<object>();
            List<object> removed = new List<object>();
            foreach (var trackedObject in this.trackedObjects.Values)
            {
                trackedObject.DetermineState();
                switch(trackedObject.State)
                {
                    case TrackedObjectState.Inserted:
                        added.Add(trackedObject.Current);
                        break;
                    case TrackedObjectState.Deleted:
                        removed.Add(trackedObject.Current);
                        break;
                    case TrackedObjectState.Modified:
                        modified.Add(trackedObject.Current);
                        break;
                }
            }

            return new ChangeSet(added, modified, removed);
        }

        /// <summary>
        /// Gets the tracked object and begins tracking it if not already.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public override TrackedObject GetTrackedObject(object obj)
        {
            TrackedObject trackedObject;
            if (!this.trackedObjects.TryGetValue(obj, out trackedObject))
                trackedObject = this.Track(null, obj);
            return trackedObject;
        }

        /// <summary>
        /// Tracks the specified original.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <param name="current">The current.</param>
        public override TrackedObject Track(Document original, object current)
        {
            var trackedObject = new TrackedObject(this.configuration, original, current);
            this.trackedObjects.Add(current, trackedObject);
            return trackedObject;
        }

        #endregion
    }
}