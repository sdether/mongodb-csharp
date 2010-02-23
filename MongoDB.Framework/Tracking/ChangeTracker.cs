using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Mapping.Visitors;

namespace MongoDB.Framework.Tracking
{
    public class ChangeTracker : IChangeTracker
    {
        #region Private Static Methods

        /// <summary>
        /// Ares the documents equal.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        private static bool AreDocumentsEqual(Document a, Document b)
        {
            if (a.Keys.Count != b.Keys.Count)
                return false;

            foreach (string key in a.Keys)
            {
                object aValue = a[key];
                object bValue = b[key];
                if (aValue == null && bValue == null)
                    continue;

                if (aValue == null && bValue != null || aValue != null && bValue == null)
                    return false;
                else if (aValue is Document && bValue is Document)
                {
                    if (!AreDocumentsEqual((Document)aValue, (Document)bValue))
                        return false;
                }
                else if (aValue is Document || bValue is Document)
                    return false;
                else if (!aValue.Equals(bValue))
                    return false;
            }

            return true;
        }

        #endregion

        #region Private Fields

        private IMongoSessionImplementor mongoSession;
        private List<TrackedEntity> trackedEntities;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeTracker"/> class.
        /// </summary>
        /// <param name="mongoSession">The mongo session.</param>
        public ChangeTracker(IMongoSessionImplementor mongoSession)
        {
            if (mongoSession == null)
                throw new ArgumentNullException("mongoSession");

            this.mongoSession = mongoSession;
            this.trackedEntities = new List<TrackedEntity>();
        }

        ~ChangeTracker()
        {
            this.Dispose(false);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets the change set.
        /// </summary>
        /// <returns></returns>
        public ChangeSet GetChangeSet()
        {
            var inserted = new List<object>();
            var modified = new List<object>();
            var deleted = new List<object>();

            foreach (var trackedEntity in this.trackedEntities)
            {
                if (trackedEntity.State == TrackedEntityState.PossiblyModified)
                    this.DetermineState(trackedEntity);

                switch (trackedEntity.State)
                {
                    case TrackedEntityState.Inserted:
                        inserted.Add(trackedEntity.Current);
                        break;
                    case TrackedEntityState.Modified:
                        modified.Add(trackedEntity.Current);
                        break;
                    case TrackedEntityState.Deleted:
                        deleted.Add(trackedEntity.Current);
                        break;
                }
            }

            return new ChangeSet(inserted, modified, deleted);
        }

        /// <summary>
        /// Gets the tracked entity, or creates one if it does not exist.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public ITrackedEntity GetTrackedEntity(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var trackedEntity = this.trackedEntities.FirstOrDefault(x => Object.Equals(x.Current, entity));
            if(trackedEntity == null)
            {
                trackedEntity = new TrackedEntity() { Current = entity };
                this.trackedEntities.Add(trackedEntity);
            }

            return trackedEntity;
        }

        /// <summary>
        /// Stops the tracking.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void StopTracking(object entity)
        {
            var trackedEntity = this.trackedEntities.FirstOrDefault(x => Object.Equals(x.Current, entity));
            if (trackedEntity != null)
                this.trackedEntities.Remove(trackedEntity);
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

            if (this.trackedEntities != null)
            {
                this.trackedEntities.Clear();
                this.trackedEntities = null;
            }
        }

        /// <summary>
        /// Determines the state.
        /// </summary>
        private void DetermineState(TrackedEntity trackedEntity)
        {
            if (trackedEntity.State != TrackedEntityState.PossiblyModified)
                return;

            var document = this.mongoSession.MapToDocument(trackedEntity.Current);

            if (trackedEntity.Original == null)
            {
                //we need to do something else, like check ids against unsaved and what-not
                throw new NotImplementedException();
            }

            if (!AreDocumentsEqual(document, trackedEntity.Original))
                trackedEntity.State = TrackedEntityState.Modified;
        }

        #endregion

        #region Private Class : TrackedEntity

        private class TrackedEntity : ITrackedEntity
        {
            public object Current { get; set; }

            public MongoDB.Driver.Document Original { get; set; }

            public TrackedEntityState State { get; set; }

            public void MoveToModified()
            {
                this.State = TrackedEntityState.Modified;
            }

            public void MoveToPossibleModified(MongoDB.Driver.Document original)
            {
                this.State = TrackedEntityState.PossiblyModified;
                this.Original = original;
            }

            public void MoveToInserted()
            {
                this.State = TrackedEntityState.Inserted;
                this.Original = null;
            }

            public void MoveToDeleted()
            {
                this.State = TrackedEntityState.Deleted;
                this.Original = null;
            }
        }

        #endregion
    }
}