using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper.Mapping
{
    public class SimpleValueType : ValueTypeBase
    {
        public IValueConverter ValueConverter { get; private set; }

        public SimpleValueType(Type type)
            : base(type)
        { }

        public SimpleValueType(Type type, IValueConverter valueConverter)
            : base(type)
        {
            if (valueConverter == null)
                throw new ArgumentNullException("valueConverter");

            this.ValueConverter = valueConverter;
        }

        public override void Accept(IMapVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
