using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping.Types;
using MongoDB.Framework.Linq.Visitors;
using MongoDB.Framework.Reflection;
using System.Reflection;
using System.Linq.Expressions;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentMemberMap<TMember> : FluentMap<MemberMap>
    {
        private MemberMap memberMap;

        public override MemberMap Instance
        {
            get { return this.memberMap; }
        }

        public FluentMemberMap(Type type)
        {
            this.memberMap.ValueType = new NullSafeValueType(type);
        }

        public FluentMemberMap<TMember> Key(string key)
        {
            this.memberMap.Key = key;
            return this;
        }

        public FluentMemberMap<TMember> NestedClass(Action<FluentNestedClassMap<TMember>> configure)
        {
            var fluentNestedClassMap = new FluentNestedClassMap<TMember>();
            this.memberMap.ValueType = new NestedClassValueType(fluentNestedClassMap.Instance);
            configure(fluentNestedClassMap);
            return this;
        }
    }
}