using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.Mapping;

namespace MongoDB.Framework.Configuration.Mapping
{
    public class ConvertibleMemberMapModel : PersistentMemberMapModel
    {
        public IValueConverter ValueConverter { get; set; }

        public override void Accept(IMapModelVisitor visitor)
        {
            visitor.ProcessMember(this);
        }
    }
}