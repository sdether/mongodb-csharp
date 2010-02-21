using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.Configuration.Fluent.Mapping.Conventions;
using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Configuration.Mapping.Conventions;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    public class FluentConventions
    {
        public MappingConventions Conventions { get; private set; }

        public FluentConventions()
            : this(new MappingConventions())
        { }

        public FluentConventions(MappingConventions model)
        {
            this.Conventions = model;
        }

        public FluentConventions Id(Action<FluentAutoId> config)
        {
            var id = new FluentAutoId(this.Conventions);
            config(id);
            return this;
        }

        public FluentConventions Map(Action<FluentMemberFinder> config)
        {
            var f = new FluentMemberFinder(this.Conventions);
            config(f);
            return this;
        }

        public void CollectionConventionIs(ICollectionConvention convention)
        {
            this.Conventions.CollectionConvention = convention;
        }

        public void CollectionNameConventionIs(ICollectionNameConvention convention)
        {
            this.Conventions.CollectionNameConvention = convention;
        }

        public void ExtendedPropertiesConventionIs(IExtendedPropertiesConvention convention)
        {
            this.Conventions.ExtendedPropertiesConvention = convention;
        }

        public void IdConventionIs(IIdConvention convention)
        {
            this.Conventions.IdConvention = convention;
        }

        public void MemberKeyConventionIs(IMemberKeyConvention convention)
        {
            this.Conventions.MemberKeyConvention = convention;
        }

        public void ValueConverterConventionIs(IValueConverterConvention convention)
        {
            this.Conventions.ValueConverterConvention = convention;
        }



    }
}