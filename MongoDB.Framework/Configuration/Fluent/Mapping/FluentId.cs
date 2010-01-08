using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Mapping;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    public class FluentId : FluentBase<IdMapModel>
    {
        public FluentGeneratedBy GeneratedBy
        {
            get { return new FluentGeneratedBy(this); }
        }

        public FluentId()
            : base(new IdMapModel())
        { }

        public FluentId UnsavedValue(object unsavedValue)
        {
            this.Model.UnsavedValue = unsavedValue;
            return this;
        }

        public FluentId CustomConverterIs<TConverter>() where TConverter : IValueConverter, new()
        {
            return this.CustomConverterIs(new TConverter());
        }

        public FluentId CustomConverterIs(IValueConverter valueConverter)
        {
            this.Model.ValueConverter = valueConverter;
            return this;
        }
    }
}