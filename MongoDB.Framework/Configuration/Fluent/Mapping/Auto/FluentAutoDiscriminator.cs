using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Configuration.Mapping.Conventions;
using MongoDB.Framework.Configuration.Mapping;

namespace MongoDB.Framework.Configuration.Fluent.Mapping.Auto
{
    public class FluentAutoDiscriminator
    {
        private AutoMapModel autoMapModel;
        private Func<Type, string> key;

        public FluentAutoDiscriminator(AutoMapModel autoMapModel)
        {
            this.autoMapModel = autoMapModel;
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

        public void ValueIs(Func<Type, object> value)
        {
            //TODO: fix with type...
            this.autoMapModel.DiscriminatorConvention = new CustomDiscriminatorConvention(t => true, t => true, key, value);
        }
    }
}