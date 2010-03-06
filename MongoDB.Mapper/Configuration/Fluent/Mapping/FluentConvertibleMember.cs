using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Mapper.Configuration.Mapping;
using MongoDB.Mapper.Mapping;
using MongoDB.Mapper.Reflection;

namespace MongoDB.Mapper.Configuration.Fluent.Mapping
{
    public class FluentConvertibleMember : FluentPersistentMember<ConvertibleMemberMapModel, FluentConvertibleMember>
    {
        private ClassMapModelBase classMapModel;

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