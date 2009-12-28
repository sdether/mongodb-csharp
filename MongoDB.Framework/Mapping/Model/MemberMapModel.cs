using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Mapping.Model
{
    public class MemberMapModel : MapModel
    {
        public MemberInfo Getter { get; set; }
        public MemberInfo Setter { get; set; }
    }
}
