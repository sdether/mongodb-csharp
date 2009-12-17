using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MongoDB.Framework
{
    public class ChangeSet
    {
        /// <summary>
        /// Gets the entities that have been added.
        /// </summary>
        /// <value>The added.</value>
        public IList<object> Added { get; private set; }

        /// <summary>
        /// Gets the entities that have been modified.
        /// </summary>
        /// <value>The modified.</value>
        public IList<object> Modified { get; private set; }

        /// <summary>
        /// Gets the entities that have been removed.
        /// </summary>
        /// <value>The removed.</value>
        public IList<object> Removed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeSet"/> class.
        /// </summary>
        /// <param name="added">The added.</param>
        /// <param name="modified">The modified.</param>
        /// <param name="removed">The removed.</param>
        public ChangeSet(IList<object> added, IList<object> modified, IList<object> removed)
        {
            if (added == null)
                throw new ArgumentNullException("inserted");
            if (modified == null)
                throw new ArgumentNullException("modified");
            if (removed == null)
                throw new ArgumentNullException("deleted");

            this.Added = new ReadOnlyCollection<object>(added);
            this.Modified = new ReadOnlyCollection<object>(modified);
            this.Removed = new ReadOnlyCollection<object>(removed);
        }
    }
}