using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Configuration.Mapping.Models;

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

        public FluentId CustomTypeIs<TValueType>() where TValueType : IValueType, new()
        {
            return this.CustomTypeIs(new TValueType());
        }

        public FluentId CustomTypeIs(IValueType valueType)
        {
            this.Model.CustomValueType = valueType;
            return this;
        }
    }
}