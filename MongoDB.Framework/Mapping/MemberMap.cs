using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public abstract class MemberMap : Map
    {
        public string Key { get; set; }

        public string MemberName { get; set; }

        public Type MemberType { get; set; }

        public Func<object, object> MemberGetter { get; set; }

        public Action<object, object> MemberSetter { get; set; }

        public bool PersistNulls { get; set; }
    }
}