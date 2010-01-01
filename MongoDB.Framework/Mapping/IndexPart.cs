using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class IndexPart
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; private set; }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>The direction.</value>
        public IndexDirection Direction { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexPart"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="direction">The direction.</param>
        public IndexPart(string key, IndexDirection direction)
        {
            if (key == null)
                throw new ArgumentException("Cannot be null or empty.", "key");
            this.Key = key;
            this.Direction = direction;
        }
    }
}
