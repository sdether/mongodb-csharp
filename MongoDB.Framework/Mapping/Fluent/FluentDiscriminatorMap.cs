using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentDiscriminatorMap<TDiscriminator>
    {
        private RootDocumentMap rootDocumentMap;

        public FluentDiscriminatorMap(RootDocumentMap rootDocumentMap)
        {
            this.rootDocumentMap = rootDocumentMap;
        }

        public void Sub<TSubEntity>(TDiscriminator discriminator, Action<FluentSubDocumentMap<TSubEntity>> configure)
        {
            var fluentSubDocumentMap = new FluentSubDocumentMap<TSubEntity>(this.rootDocumentMap);
            configure(fluentSubDocumentMap);
            fluentSubDocumentMap.Instance.Discriminator = discriminator;
            this.rootDocumentMap.AddSubDocumentMap(fluentSubDocumentMap.Instance);
        }
    }
}