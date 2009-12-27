using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Linq.Visitors;
using MongoDB.Framework.Reflection;
using MongoDB.Framework.Mapping.Types;

namespace MongoDB.Framework.Mapping.Fluent
{
    public abstract class FluentClassMap<TMap, TEntity> : FluentMap<TMap> where TMap : ClassMap
    {
        public FluentMemberMap Map(string memberName)
        {
            var memberInfo = this.GetSingleMember(memberName);
            return this.Map(memberInfo);
        }

        public FluentMemberMap Map(MemberInfo memberInfo)
        {
            var memberType = LateBoundReflection.GetMemberValueType(memberInfo);
            var fluentMemberMap = new FluentMemberMap();
            fluentMemberMap.Instance.Key = memberInfo.Name;
            fluentMemberMap.Instance.MemberName = memberInfo.Name;
            fluentMemberMap.Instance.MemberGetter = LateBoundReflection.GetGetter(memberInfo);
            fluentMemberMap.Instance.MemberSetter = LateBoundReflection.GetSetter(memberInfo);
            fluentMemberMap.Instance.MemberType = memberType;
            fluentMemberMap.Instance.ValueType = new NullSafeValueType(memberType);

            this.Instance.AddMemberMap(fluentMemberMap.Instance);
            return fluentMemberMap;
        }

        public FluentMemberMap Map(Expression<Func<TEntity, object>> member)
        {
            var memberInfo = this.GetSingleMember(member);
            return this.Map(memberInfo);
        }

        public FluentMemberMap NestedClass<TNestedClass>(string memberName, Action<FluentNestedClassMap<TNestedClass>> configure)
        {
            var memberInfo = this.GetSingleMember(memberName);
            return this.NestedClass<TNestedClass>(memberInfo, configure);
        }

        public FluentMemberMap NestedClass<TNestedClass>(MemberInfo memberInfo, Action<FluentNestedClassMap<TNestedClass>> configure)
        {
            var fluentNestedClassMap = new FluentNestedClassMap<TNestedClass>();
            var fluentMemberMap = new FluentMemberMap();
            fluentMemberMap.Instance.Key = memberInfo.Name;
            fluentMemberMap.Instance.MemberName = memberInfo.Name;
            fluentMemberMap.Instance.MemberGetter = LateBoundReflection.GetGetter(memberInfo);
            fluentMemberMap.Instance.MemberSetter = LateBoundReflection.GetSetter(memberInfo);
            fluentMemberMap.Instance.MemberType = typeof(TNestedClass);
            fluentMemberMap.Instance.ValueType = new NestedClassValueType(fluentNestedClassMap.Instance);

            this.Instance.AddMemberMap(fluentMemberMap.Instance);
            configure(fluentNestedClassMap);
            return fluentMemberMap;
        }

        public FluentMemberMap NestedClass<TNestedClass>(Expression<Func<TEntity, TNestedClass>> member, Action<FluentNestedClassMap<TNestedClass>> configure)
        {
            var memberInfo = this.GetSingleMember(member);
            return this.NestedClass<TNestedClass>(memberInfo, configure);
        }

        protected MemberInfo GetSingleMember(string memberName)
        {
            var members = typeof(TEntity).GetMember(memberName);
            if (members.Length > 1)
                throw new InvalidOperationException(string.Format("More than one member found with memberName {0}.", memberName));

            return members[0];
        }

        protected MemberInfo GetSingleMember<TMember>(Expression<Func<TEntity, TMember>> member)
        {
            var members = MemberInfoPathBuilder.BuildFrom(member);
            if (members.Count > 1)
                throw new InvalidOperationException("Only top-level members are supported for ids.");

            return members[0];
        }
    }
}
