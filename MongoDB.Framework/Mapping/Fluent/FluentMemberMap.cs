using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Fluent
{
    public abstract class FluentMemberMap<TMap> : FluentMap<TMap> where TMap : MemberMap
    {
        public FluentMemberMap<TMap> Key(string key)
        {
            this.Instance.Key = key;
            return this;
        }
    }
}