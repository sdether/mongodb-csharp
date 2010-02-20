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
            get { return new FluentAutoDiscriminator(this.Model); }
        }

        public FluentAutoId Id
        {
            get { return new FluentAutoId(this.Model); }
        }

        public FluentAutoMemberFinder Map
        {
            get { return new FluentAutoMemberFinder(this.Model); }
        }

        public FluentAutoMap(AutoMapModel model)
        {
            this.Model = model;
        }

        public void CollectionConventionIs(ICollectionConvention convention)
        {
            this.Model.CollectionConvention = convention;
        }

        public void CollectionNameConventionIs(ICollectionNameConvention convention)
        {
            this.Model.CollectionNameConvention = convention;
        }

        public void DiscriminatorConventionIs(IDiscriminatorConvention convention)
        {
            this.Model.DiscriminatorConvention = convention;
        }

        public void ExtendedPropertiesConventionIs(IExtendedPropertiesConvention convention)
        {
            this.Model.ExtendedPropertiesConvention = convention;
        }

        public void IdConventionIs(IIdConvention convention)
        {
            this.Model.IdConvention = convention;
        }

        public void MemberKeyConventionIs(IMemberKeyConvention convention)
        {
            this.Model.MemberKeyConvention = convention;
        }

        public void ValueConverterConventionIs(IValueConverterConvention convention)
        {
            this.Model.ValueConverterConvention = convention;
        }



    }
}