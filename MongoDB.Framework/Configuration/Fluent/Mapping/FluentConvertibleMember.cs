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
    public class FluentConvertibleMember : FluentPersistentMember<ConvertibleMemberMapModel, FluentConvertibleMember>
    {
        private ClassMapModel classMapModel;

        protected override FluentConvertibleMember Fluent
        {
            get { return this; }
        }

        public FluentConvertibleMember()
            : base(new ConvertibleMemberMapModel())
        { }

        public FluentConvertibleMember ConvertWith<TConverter>() where TConverter : IValueConverter, new()
        {
            return this.ConvertWith(new TConverter());
        }

        public FluentConvertibleMember ConvertWith(IValueConverter valueConverter)
        {
            this.Model.ValueConverter = valueConverter;
            return this;
        }
    }
}