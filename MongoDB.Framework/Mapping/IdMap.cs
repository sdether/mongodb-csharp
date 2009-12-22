using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping
{
    public class IdMap : SimpleValueMap
    {
        public IdMap(string memberName, Type memberType, Func<object, object> memberGetter, Action<object, object> memberSetter)
            : base("_id", memberName, memberType, memberGetter, memberSetter)
        {
            this.PersistNulls = false;
        }
    }
}