using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentDiscriminatorMap<TDiscriminator>
    {
        private SuperClassMap rootClassMap;

        public FluentDiscriminatorMap(SuperClassMap rootClassMap)
        {
            this.rootClassMap = rootClassMap;
        }

        public FluentDiscriminatorMap<TDiscriminator> SubClass<T>(TDiscriminator discriminator, Action<FluentSubClassMap<T>> configure)
        {
            var fluentSubClassMap = new FluentSubClassMap<T>(this.rootClassMap);
            configure(fluentSubClassMap);
            fluentSubClassMap.Instance.Discriminator = discriminator;
            this.rootClassMap.AddSubClassMap(fluentSubClassMap.Instance);
            return this;
        }
    }
}