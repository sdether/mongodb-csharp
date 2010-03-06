using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper.Mapping
{
    public class ManyToOneValueType : ValueTypeBase
    {
        /// <summary>
        /// Gets a value indicating whether this instance is lazy.
        /// </summary>
        /// <value><c>true</c> if this instance is lazy; otherwise, <c>false</c>.</value>
        public bool IsLazy { get; private set; }

        /// <summary>
        /// Gets the type of the reference.
        /// </summary>
        /// <value>The type of the reference.</value>
        public Type ReferenceType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManyToOneValueType"/> class.
        /// </summary>
        /// <param name="referenceType">Type of the reference.</param>
        public ManyToOneValueType(Type referenceType, bool isLazy)
            : base(referenceType)
        {
            if (referenceType == null)
                throw new ArgumentNullException("referenceType");

            this.IsLazy = isLazy;
            this.ReferenceType = referenceType;
        }

        public override void Accept(IMapVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
