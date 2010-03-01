using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class NestedClassValueType : ValueTypeBase
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the value converter.
        /// </summary>
        /// <value>The value converter.</value>
        public IValueConverter ValueConverter { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NestedClassValueType"/> class.
        /// </summary>
        /// <param name="classMap">The class map.</param>
        /// <param name="valueConverter">The value converter.</param>
        public NestedClassValueType(Type type, IValueConverter valueConverter)
            : base(type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (valueConverter == null)
                throw new ArgumentNullException("valueConverter");

            this.Type = type;
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
