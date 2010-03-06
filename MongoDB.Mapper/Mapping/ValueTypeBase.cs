using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper.Mapping
{
    public abstract class ValueTypeBase : MapNode
    {
        public Type Type { get; private set; }

        protected ValueTypeBase(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            this.Type = type;
        }
    }
}