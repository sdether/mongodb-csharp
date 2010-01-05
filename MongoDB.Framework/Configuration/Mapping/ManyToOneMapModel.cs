using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping
{
    public class ManyToOneMapModel : MemberMapModelBase
    {
        public bool IsLazy { get; set; }

        public ManyToOneMapModel()
        {
            this.IsLazy = true;
        }

        public override void Accept(IMapModelVisitor visitor)
        {
            visitor.ProcessManyToOne(this);
        }
    }
}