using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping
{
    public class NestedClassMapModel : SuperClassMapModel
    {
        public NestedClassMapModel(Type type)
            : base(type)
        { }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IMapModelVisitor visitor)
        {
            visitor.ProcessNestedClass(this);

            base.Accept(visitor);
        }
    }
}
