using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Persistence;

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
                else if (aValue is Document && bValue is Document)
                {
                    if(!AreDocumentsEqual((Document)aValue, (Document)bValue))
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

        private MappingStore mappingStore;

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
        /// <param name="configuration">The configuration.</param>
        /// <param name="original">The original.</param>
        /// <param name="current">The current.</param>
        public TrackedObject(MappingStore mappingStore, Document original, object current)
        {
            if (mappingStore == null)
                throw new ArgumentNullException("mappingStore");
            if (current == null)
                throw new ArgumentNullException("current");

            this.mappingStore = mappingStore;
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

            var classMap = this.mappingStore.GetClassMapFor(this.Current.GetType());
            if (this.Original == null)
            {
                var value = (string)this.GetId();
                if (value == null || string.IsNullOrEmpty(value))
                    this.MoveToInserted();
                else
                    this.MoveToModified();
                return;
            }

            throw new NotImplementedException();
            //var translator = new EntityToDocumentTranslator(this.mappingStore);
            //var document = translator.Translate(this.Current);
            //if (!AreDocumentsEqual(this.Original, document))
            //    this.MoveToModified();
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <returns></returns>
        public string GetId()
        {
            return (string)this.mappingStore.GetClassMapFor(this.Current.GetType()).IdMap.MemberGetter(this.Current);
        }

        /// <summary>
        /// Moves to inserted.
        /// </summary>
        public void MoveToInserted()
        {
            this.Original = null;
            this.State = TrackedObjectState.Inserted;
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
        /// Moves to deleted.
        /// </summary>
        public void MoveToDeleted()
        {
            this.Original = null;
            this.State = TrackedObjectState.Deleted;
        }

        #endregion
    }
}
