using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class CollectionValueType : ValueTypeBase
    {
        /// <summary>
        /// Gets the type of the collection.
        /// </summary>
        /// <value>The type of the collection.</value>
        public ICollectionType CollectionType { get; private set; }

        /// <summary>
        /// Gets the type of the element value.
        /// </summary>
        /// <value>The type of the element value.</value>
        public ValueTypeBase ElementValueType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionValueType"/> class.
        /// </summary>
        /// <param name="collectionType">Type of the collection.</param>
        /// <param name="elementValueType">Type of the element value.</param>
        public CollectionValueType(ICollectionType collectionType, ValueTypeBase elementValueType)
            : base(collectionType.GetCollectionType(elementValueType.Type))
        {
            if (collectionType == null)
                throw new ArgumentNullException("collectionType");
            if (elementValueType == null)
                throw new ArgumentNullException("elementValueType");

            this.CollectionType = collectionType;
            this.ElementValueType = elementValueType;
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
