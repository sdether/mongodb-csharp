using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping
{
    public abstract class MemberMapModelBase : ModelNode
    {
        public string Key { get; set; }

        public MemberInfo Getter { get; set; }

        public MemberInfo Setter { get; set; }

        public bool PersistNull { get; set; }
    }
}
