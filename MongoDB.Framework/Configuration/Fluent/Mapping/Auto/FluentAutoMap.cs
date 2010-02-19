using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Configuration.Mapping.Conventions;

namespace MongoDB.Framework.Configuration.Fluent.Mapping.Auto
{
    public class FluentAutoMap
    {
        public AutoMapModel Model { get; private set; }

        public FluentAutoDiscriminator Discriminator
        {
            get { return new FluentAutoDiscriminator(this); }
        }

        public FluentAutoMap()
        {
            this.Model = new AutoMapModel();
        }

        public void KeyConventionIs(IMemberKeyConvention keyConvention)
        {
            this.Model.MemberKeyConvention = keyConvention;
        }
    }
}