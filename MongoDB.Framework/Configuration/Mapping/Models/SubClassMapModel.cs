using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping.Models
{
    public class SubClassMapModel : ClassMapModel
    {
        public SubClassMapModel(Type type)
            : base(type)
        { }

        public override void Accept(IMapModelVisitor visitor)
        {
            visitor.ProcessSubClass(this);

            base.Accept(visitor);
        }
    }
}