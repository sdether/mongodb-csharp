using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentSimpleValueMap : FluentValueMap<SimpleValueMap>
    {
        private SimpleValueMap instance;

        public override SimpleValueMap Instance
        {
            get { return this.instance; }
        }

        public FluentSimpleValueMap()
        {
            this.instance = new SimpleValueMap();
        }
    }
}