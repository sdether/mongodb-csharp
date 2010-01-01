using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.Mapping.Models;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentIdMap : FluentBase<IdMapModel>
    {
        public FluentGeneratedBy GeneratedBy
        {
            get { return new FluentGeneratedBy(this); }
        }

        public FluentIdMap()
            : base(new IdMapModel())
        { }

        public FluentIdMap UnsavedValue(object unsavedValue)
        {
            this.Model.UnsavedValue = unsavedValue;
            return this;
        }

        public FluentIdMap CustomTypeIs<TValueType>() where TValueType : IValueType, new()
        {
            return this.CustomTypeIs(new TValueType());
        }

        public FluentIdMap CustomTypeIs(IValueType valueType)
        {
            this.Model.CustomValueType = valueType;
            return this;
        }
    }
}