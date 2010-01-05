using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.Mapping;

namespace MongoDB.Framework.Configuration.Mapping.Models
{
    public class ValueMapModel : MemberMapModelBase
    {
        public IValueType CustomValueType { get; set; }

        public override void Accept(IMapModelVisitor visitor)
        {
            visitor.ProcessValue(this);
        }
    }
}