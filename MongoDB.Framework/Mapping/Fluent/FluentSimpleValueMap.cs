using System;
using System.Collections.Generic;
using System.Linq;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentSimpleValueMap : FluentValueMap<SimpleValueMap>
    {
        private readonly SimpleValueMap instance;

        public FluentSimpleValueMap()
        {
            this.instance = new SimpleValueMap();
        }

        public override SimpleValueMap Instance
        {
            get { return this.instance; }
        }
    }
}