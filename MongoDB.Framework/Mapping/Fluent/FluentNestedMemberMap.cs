using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentNestedMemberMap<T> : FluentMemberMap<NestedClassMemberMap>
    {
        private readonly NestedClassMemberMap instance;
        private readonly FluentNestedClassMap<T> fluentNestedClassMap;

        public override NestedClassMemberMap Instance
        {
            get { return this.instance; }
        }

        public FluentNestedMemberMap(FluentNestedClassMap<T> fluentNestedClassMap)
        {
            this.fluentNestedClassMap = fluentNestedClassMap;
            this.instance = new NestedClassMemberMap(fluentNestedClassMap.Instance);
        }

        public FluentNestedMemberMap<T> Configure(Action<FluentNestedClassMap<T>> configure)
        {
            configure(this.fluentNestedClassMap);
            return this;
        }
    }
}