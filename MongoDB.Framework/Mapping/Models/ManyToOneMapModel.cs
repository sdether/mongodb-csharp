using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Models
{
    public class ManyToOneMapModel : MemberMapModelBase
    {
        public override void Accept(IMapModelVisitor visitor)
        {
            visitor.ProcessManyToOne(this);
        }
    }
}