using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Mapping.Models;
using MongoDB.Framework.Mapping.Types;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentValueMap : FluentMap<ValueMapModel>
    {
        public FluentValueMap()
            : base(new ValueMapModel())
        { }

        public FluentValueMap Key(string key)
        {
            this.Model.Key = key;
            return this;
        }

        public FluentValueMap CustomTypeIs<TValueType>() where TValueType : IValueType, new()
        {
            return this.CustomTypeIs(new TValueType());
        }

        public FluentValueMap CustomTypeIs(IValueType valueType)
        {
            this.Model.CustomValueType = valueType;
            return this;
        }
    }
}