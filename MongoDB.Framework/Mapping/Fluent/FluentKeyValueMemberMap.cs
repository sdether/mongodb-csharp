using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Mapping.Model;
using MongoDB.Framework.Mapping.Types;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentKeyValueMemberMap : FluentMemberMap<KeyValueMemberMapModel>
    {
        public FluentKeyValueMemberMap()
            : base(new KeyValueMemberMapModel())
        { }

        public FluentKeyValueMemberMap Key(string key)
        {
            this.Model.Key = key;
            return this;
        }

        public FluentKeyValueMemberMap CustomTypeIs<TValueType>() where TValueType : IValueType, new()
        {
            return this.CustomTypeIs(new TValueType());
        }

        public FluentKeyValueMemberMap CustomTypeIs(IValueType valueType)
        {
            this.Model.CustomValueType = valueType;
            return this;
        }
    }
}