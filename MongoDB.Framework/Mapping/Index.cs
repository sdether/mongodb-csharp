using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping
{
    public class Index : Map
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is unique.
        /// </summary>
        /// <value><c>true</c> if this instance is unique; otherwise, <c>false</c>.</value>
        public bool IsUnique { get; private set; }

        /// <summary>
        /// Gets or sets the parts.
        /// </summary>
        /// <value>The parts.</value>
        public IEnumerable<KeyValuePair<string, IndexDirection>> Parts { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexMap"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="indexParts">The index parts.</param>
        /// <param name="isUnique">if set to <c>true</c> [is unique].</param>
        public Index(string name, IEnumerable<KeyValuePair<string, IndexDirection>> indexParts, bool isUnique)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (indexParts == null)
                throw new ArgumentNullException("indexParts");

            this.Name = name;
            this.Parts = indexParts;
            this.IsUnique = isUnique;
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IMapVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}