using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MongoDB.Mapper.Configuration.Mapping
{
    public abstract class MemberMapModel : ModelNode
    {
        public MemberInfo Getter { get; set; }

        public MemberInfo Setter { get; set; }
    }
}
