using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class NestedClassValueType : ValueTypeBase
    {
        /// <summary>
        /// Gets the nested class map.
        /// </summary>
        /// <value>The nested class map.</value>
        public NestedClassMap NestedClassMap { get; private set; }

        /// <summary>
        /// Gets or sets the value converter.
        /// </summary>
        /// <value>The value converter.</value>
        public IValueConverter ValueConverter { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NestedClassValueType"/> class.
        /// </summary>
        /// <param name="nestedClassMap">The nested class map.</param>
        public NestedClassValueType(NestedClassMap nestedClassMap, IValueConverter valueConverter)
            : base(nestedClassMap.Type)
        {
            if (nestedClassMap == null)
                throw new ArgumentNullException("nestedClassMap");
            if (valueConverter == null)
                throw new ArgumentNullException("valueConverter");

            this.NestedClassMap = nestedClassMap;
            this.ValueConverter = valueConverter;
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
