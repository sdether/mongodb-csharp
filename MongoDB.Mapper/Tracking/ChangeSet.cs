using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper.Tracking
{
    public sealed class ChangeSet
    {
        /// <summary>
        /// Gets the inserted.
        /// </summary>
        /// <value>The inserted.</value>
        public IEnumerable<object> Inserted { get; private set; }

        /// <summary>
        /// Gets the modified.
        /// </summary>
        /// <value>The modified.</value>
        public IEnumerable<object> Modified { get; private set; }

        /// <summary>
        /// Gets the deleted.
        /// </summary>
        /// <value>The deleted.</value>
        public IEnumerable<object> Deleted { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeSet"/> class.
        /// </summary>
        /// <param name="inserted">The inserted.</param>
        /// <param name="modified">The modified.</param>
        /// <param name="deleted">The deleted.</param>
        public ChangeSet(IEnumerable<object> inserted, IEnumerable<object> modified, IEnumerable<object> deleted)
        {
            this.Inserted = new List<object>(inserted ?? Enumerable.Empty<object>());
            this.Modified = new List<object>(modified ?? Enumerable.Empty<object>());
            this.Deleted = new List<object>(deleted ?? Enumerable.Empty<object>());
        }
    }
}