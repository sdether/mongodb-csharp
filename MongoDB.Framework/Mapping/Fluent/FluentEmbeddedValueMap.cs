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
    public class FluentEmbeddedValueMap : FluentBase<EmbeddedValueMapModel>
    {
        public FluentEmbeddedValueMap()
            : base(new EmbeddedValueMapModel())
        { }

        public FluentEmbeddedValueMap Key(string key)
        {
            this.Model.Key = key;
            return this;
        }

        public FluentEmbeddedValueMap CustomTypeIs<TValueType>() where TValueType : IValueType, new()
        {
            return this.CustomTypeIs(new TValueType());
        }

        public FluentEmbeddedValueMap CustomTypeIs(IValueType valueType)
        {
            this.Model.CustomValueType = valueType;
            return this;
        }
    }
}