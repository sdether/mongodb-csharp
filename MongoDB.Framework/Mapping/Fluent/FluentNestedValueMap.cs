using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentNestedValueMap<T> : FluentValueMap<NestedClassValueMap>
    {
        private readonly NestedClassValueMap instance;
        private readonly FluentNestedClassMap<T> fluentNestedClassMap;

        public override NestedClassValueMap Instance
        {
            get { return this.instance; }
        }

        public FluentNestedValueMap(FluentNestedClassMap<T> fluentNestedClassMap)
        {
            this.fluentNestedClassMap = fluentNestedClassMap;
            this.instance = new NestedClassValueMap(fluentNestedClassMap.Instance);
        }

        public FluentNestedValueMap<T> Configure(Action<FluentNestedClassMap<T>> configure)
        {
            configure(this.fluentNestedClassMap);
            return this;
        }
    }
}