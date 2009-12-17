using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Tracking
{
    public class TrackedObject
    {
        #region Private Static Methods

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
                else if (aValue is Document && bValue is Document && !AreDocumentsEqual((Document)aValue, (Document)bValue))
                {
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

        private EntityMapper entityMapper;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public TrackedObjectState State { get; private set; }

        /// <summary>
        /// Gets or sets the original.
        /// </summary>
        /// <value>The original.</value>
        public Document Original { get; private set; }

        /// <summary>
        /// Gets or sets the current.
        /// </summary>
        /// <value>The current.</value>
        public object Current { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackedObject"/> class.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <param name="current">The current.</param>
        /// <param name="entityMapper">The entity mapper.</param>
        public TrackedObject(EntityMapper entityMapper, Document original, object current)
        {
            if (entityMapper == null)
                throw new ArgumentNullException("entityMapper");
            if (current == null)
                throw new ArgumentNullException("current");

            this.entityMapper = entityMapper;
            this.Original = original;
            this.Current = current;
            this.State = TrackedObjectState.PossiblyModified;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether this instance is modified.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is modified; otherwise, <c>false</c>.
        /// </returns>
        public void DetermineState()
        {
            if (this.State != TrackedObjectState.PossiblyModified)
                return;

            if (this.Original == null)
            {
                var rootEntityMap = this.entityMapper.Configuration.GetRootEntityMapFor(this.Current.GetType());
                var value = (string)rootEntityMap.IdMap.Getter(this.Current);
                if (rootEntityMap.IdMap.TransientValues.Contains(value))
                    this.MoveToAdded();
                else
                    this.MoveToModified();
            }

            Document current = this.entityMapper.MapEntityToDocument(this.Current);
            if (!AreDocumentsEqual(this.Original, current))
                this.MoveToModified();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Moves to added.
        /// </summary>
        public void MoveToAdded()
        {
            this.Original = null;
            this.State = TrackedObjectState.Added;
        }

        /// <summary>
        /// Moves to dead.
        /// </summary>
        public void MoveToDead()
        {
            this.Original = null;
            this.Current = null;
            this.State = TrackedObjectState.Dead;
        }

        /// <summary>
        /// Moves to modified.
        /// </summary>
        public void MoveToModified()
        {
            this.State = TrackedObjectState.Modified;
        }

        /// <summary>
        /// Moves to possible modified.
        /// </summary>
        /// <param name="original">The original.</param>
        public void MoveToPossibleModified(Document original)
        {
            this.Original = original;
            this.State = TrackedObjectState.PossiblyModified;
        }

        /// <summary>
        /// Moves to removed.
        /// </summary>
        public void MoveToRemoved()
        {
            this.Original = null;
            this.State = TrackedObjectState.Removed;
        }

        #endregion
    }
}
