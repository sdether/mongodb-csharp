using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping
{
    public class IndexMap : Map
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is unique.
        /// </summary>
        /// <value><c>true</c> if this instance is unique; otherwise, <c>false</c>.</value>
        public bool IsUnique { get; private set; }

        /// <summary>
        /// Gets or sets the parts.
        /// </summary>
        /// <value>The parts.</value>
        public IEnumerable<IndexPart> Parts { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexMap"/> class.
        /// </summary>
        /// <param name="indexParts">The index parts.</param>
        /// <param name="isUnique">if set to <c>true</c> [is unique].</param>
        public IndexMap(IEnumerable<IndexPart> indexParts, bool isUnique)
        {
            if (indexParts == null)
                throw new ArgumentNullException("indexParts");

            this.Parts = indexParts;
            this.IsUnique = isUnique;
        }

        public override void Accept(IMapVisitor visitor)
        {
            visitor.ProcessIndex(this);
        }
    }
}