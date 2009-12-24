using System;
using System.Collections.Generic;
using System.Linq;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentNestedClassMap<T> : FluentSuperClassMap<NestedClassMap, T>
    {
        private readonly NestedClassMap instance;

        public FluentNestedClassMap()
        {
            this.instance = new NestedClassMap(typeof (T));
        }

        public override NestedClassMap Instance
        {
            get { return this.instance; }
        }
    }
}