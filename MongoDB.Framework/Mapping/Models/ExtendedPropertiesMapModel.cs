using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MongoDB.Framework.Mapping.Models
{
    public class ExtendedPropertiesMapModel : MapModel
    {
        public MemberInfo Getter { get; set; }

        public MemberInfo Setter { get; set; }

        public override void Accept(IMapModelVisitor visitor)
        {
            visitor.ProcessExtendedProperties(this);
        }
    }
}