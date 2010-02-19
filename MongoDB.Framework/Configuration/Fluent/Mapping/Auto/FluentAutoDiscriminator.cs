using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Configuration.Mapping.Conventions;

namespace MongoDB.Framework.Configuration.Fluent.Mapping.Auto
{
    public class FluentAutoDiscriminator
    {
        private FluentAutoMap fluentAutoMap;
        private Func<Type, string> key;

        public FluentAutoDiscriminator(FluentAutoMap fluentAutoMap)
        {
            this.fluentAutoMap = fluentAutoMap;
        }

        public FluentAutoDiscriminator KeyIs(string key)
        {
            return this.KeyIs(t => key);
        }

        public FluentAutoDiscriminator KeyIs(Func<Type, string> key)
        {
            this.key = key;
            return this;
        }

        public FluentAutoMap AndValueIs(Func<Type, object> value)
        {
            //TODP: fix with type...
            this.fluentAutoMap.Model.DiscriminatorConvention = new CustomDiscriminatorConvention(t => true, t => true, key, value);
            return fluentAutoMap;
        }
    }
}