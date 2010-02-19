using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping
{
    public class ExtendedPropertiesMapModel : ModelNode
    {
        public MemberInfo Getter { get; set; }

        public MemberInfo Setter { get; set; }
    }
}