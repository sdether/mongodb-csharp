using System;
using System.Collections.Generic;
using System.Linq;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentNestedClassMap<TEntity> : FluentSuperClassMap<NestedClassMap, TEntity>
    {
        private readonly NestedClassMap instance;

        public FluentNestedClassMap()
        {
            this.instance = new NestedClassMap(typeof (TEntity));
        }

        public override NestedClassMap Instance
        {
            get { return this.instance; }
        }
    }
}