using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Mapping.Models
{
    public class EmbeddedValueMapModel : MemberMapModel
    {
        public IValueType CustomValueType { get; set; }

        public override void Accept(IMapModelVisitor visitor)
        {
            visitor.ProcessValue(this);

            base.Accept(visitor);
        }
    }
}