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
        /// Gets the entities that have been inserted.
        /// </summary>
        /// <value>The entities.</value>
        public IList<object> Inserted { get; private set; }

        /// <summary>
        /// Gets the entities that have been modified.
        /// </summary>
        /// <value>The entities.</value>
        public IList<object> Modified { get; private set; }

        /// <summary>
        /// Gets the entities that have been deleted.
        /// </summary>
        /// <value>The entities.</value>
        public IList<object> Deleted { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeSet"/> class.
        /// </summary>
        /// <param name="inserted">The inserted.</param>
        /// <param name="modified">The modified.</param>
        /// <param name="deleted">The deleted.</param>
        public ChangeSet(IList<object> inserted, IList<object> modified, IList<object> deleted)
        {
            if (inserted == null)
                throw new ArgumentNullException("inserted");
            if (modified == null)
                throw new ArgumentNullException("modified");
            if (deleted == null)
                throw new ArgumentNullException("deleted");

            this.Inserted = new ReadOnlyCollection<object>(inserted);
            this.Modified = new ReadOnlyCollection<object>(modified);
            this.Deleted = new ReadOnlyCollection<object>(deleted);
        }
    }
}