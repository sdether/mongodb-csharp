using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Fluent
{
    public abstract class FluentValueMap<TMap> : FluentMap<TMap> where TMap : ValueMap
    {
        public FluentValueMap<TMap> Key(string key)
        {
            this.Instance.Key = key;
            return this;
        }
    }
}