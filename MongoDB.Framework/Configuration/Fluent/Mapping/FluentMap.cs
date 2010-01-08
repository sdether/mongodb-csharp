using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    public class FluentMap : FluentMember<MemberMapModel, FluentMap>
    {
        protected override FluentMap Fluent
        {
            get { return this; }
        }

        public FluentMap()
            : base(new MemberMapModel())
        { }

        public FluentMap CustomConverterIs<TConverter>() where TConverter : IValueConverter, new()
        {
            return this.CustomConverterIs(new TConverter());
        }

        public FluentMap CustomConverterIs(IValueConverter valueConverter)
        {
            this.Model.ValueConverter = valueConverter;
            return this;
        }
    }
}