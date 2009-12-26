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
    public abstract class FluentClassMap<TMap, T> : FluentMap<TMap> where TMap : ClassMap
    {
        public FluentMemberMap<T> Map(string memberName)
        {
            var memberInfo = this.GetSingleMember(memberName);
            return this.Map(memberInfo);
        }

        public FluentMemberMap<T> Map(MemberInfo memberInfo)
        {
            var fluentMemberMap = new FluentMemberMap<T>(LateBoundReflection.GetMemberValueType(memberInfo));
            fluentMemberMap.Instance.Key = memberInfo.Name;
            fluentMemberMap.Instance.MemberName = memberInfo.Name;
            fluentMemberMap.Instance.MemberGetter = LateBoundReflection.GetGetter(memberInfo);
            fluentMemberMap.Instance.MemberSetter = LateBoundReflection.GetSetter(memberInfo);

            this.Instance.AddMemberMap(fluentMemberMap.Instance);
            return fluentMemberMap;
        }

        public FluentMemberMap<T> Map(Expression<Func<T, object>> member)
        {
            var memberInfo = this.GetSingleMember(member);
            return this.Map(memberInfo);
        }

        public FluentMemberMap<T> NestedClass<TNestedClass>(string memberName, Action<FluentNestedClassMap<TNestedClass>> configure)
        {
            var memberInfo = this.GetSingleMember(memberName);
            return this.NestedClass<TNestedClass>(memberInfo, configure);
        }

        public FluentMemberMap<T> NestedClass<TNestedClass>(MemberInfo memberInfo, Action<FluentNestedClassMap<TNestedClass>> configure)
        {
            var fluentNestedClassMap = new FluentNestedClassMap<TNestedClass>();
            var fluentMemberMap = new FluentMemberMap<T>(typeof(TNestedClass));
            fluentMemberMap.Instance.Key = memberInfo.Name;
            fluentMemberMap.Instance.MemberName = memberInfo.Name;
            fluentMemberMap.Instance.ValueType = new NestedClassValueType(fluentNestedClassMap.Instance);
            fluentMemberMap.Instance.MemberGetter = LateBoundReflection.GetGetter(memberInfo);
            fluentMemberMap.Instance.MemberSetter = LateBoundReflection.GetSetter(memberInfo);
            this.Instance.AddMemberMap(fluentMemberMap.Instance);
            configure(fluentNestedClassMap);
            return fluentMemberMap;
        }

        public FluentMemberMap<T> NestedClass<TNestedClass>(Expression<Func<T, TNestedClass>> member, Action<FluentNestedClassMap<TNestedClass>> configure)
        {
            var memberInfo = this.GetSingleMember(member);
            return this.NestedClass<TNestedClass>(memberInfo, configure);
        }

        protected MemberInfo GetSingleMember(string memberName)
        {
            var members = typeof(T).GetMember(memberName);
            if (members.Length > 1)
                throw new InvalidOperationException(string.Format("More than one member found with memberName {0}.", memberName));

            return members[0];
        }

        protected MemberInfo GetSingleMember<TMember>(Expression<Func<T, TMember>> member)
        {
            var members = MemberInfoPathBuilder.BuildFrom(member);
            if (members.Count > 1)
                throw new InvalidOperationException("Only top-level members are supported for ids.");

            return members[0];
        }
    }
}
