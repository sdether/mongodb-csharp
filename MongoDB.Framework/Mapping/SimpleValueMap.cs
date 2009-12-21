using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class SimpleValueMap : ValueMap
    {
        public SimpleValueMap(string key, string memberName, Type memberType, Func<object, object> memberGetter, Action<object, object> memberSetter)
            : base(key, memberName, memberType, memberGetter, memberSetter)
        { }
    }
}