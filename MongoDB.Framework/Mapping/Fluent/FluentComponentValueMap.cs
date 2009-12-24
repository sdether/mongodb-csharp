using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentComponentValueMap<T> : FluentValueMap<ComponentValueMap>
    {
        private ComponentValueMap instance;
        private FluentComponentClassMap<T> fluentComponentClassMap;

        public override ComponentValueMap Instance
        {
            get { return this.instance; }
        }

        public FluentComponentValueMap(FluentComponentClassMap<T> fluentComponentClassMap)
        {
            this.fluentComponentClassMap = fluentComponentClassMap;
            this.instance = new ComponentValueMap(fluentComponentClassMap.Instance);
        }

        public FluentComponentValueMap<T> Configure(Action<FluentComponentClassMap<T>> configure)
        {
            configure(this.fluentComponentClassMap);
            return this;
        }
    }
}