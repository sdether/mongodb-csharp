using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Linq.Visitors;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Mapping.Fluent
{
    public abstract class FluentClassMap<TMap, TEntity> : FluentMap<TMap> where TMap : ClassMap
    {
        public FluentNestedMemberMap<TNestedClass> NestedClass<TNestedClass>(string memberName)
        {
            var memberInfo = this.GetSingleMember(memberName);
            return this.NestedClass<TNestedClass>(memberInfo);
        }

        public FluentNestedMemberMap<TNestedClass> NestedClass<TNestedClass>(MemberInfo memberInfo)
        {
            var fluentNestedClassMap = new FluentNestedClassMap<TNestedClass>();
            var fluentNestedClassMemberMap = new FluentNestedMemberMap<TNestedClass>(fluentNestedClassMap);
            fluentNestedClassMemberMap.Instance.Key = memberInfo.Name;
            fluentNestedClassMemberMap.Instance.MemberName = memberInfo.Name;
            fluentNestedClassMemberMap.Instance.MemberType = LateBoundReflection.GetMemberValueType(memberInfo);
            fluentNestedClassMemberMap.Instance.MemberGetter = LateBoundReflection.GetGetter(memberInfo);
            fluentNestedClassMemberMap.Instance.MemberSetter = LateBoundReflection.GetSetter(memberInfo);
            this.Instance.AddNestedClassMemberMap(fluentNestedClassMemberMap.Instance);
            return fluentNestedClassMemberMap;
        }

        public FluentNestedMemberMap<TNestedClass> NestedClass<TNestedClass>(Expression<Func<TEntity, TNestedClass>> member)
        {
            var memberInfo = this.GetSingleMember(member);
            return this.NestedClass<TNestedClass>(memberInfo);
        }

        public FluentSimpleMemberMap Map(string memberName)
        {
            var memberInfo = this.GetSingleMember(memberName);
            return this.Map(memberInfo);
        }

        public FluentSimpleMemberMap Map(MemberInfo memberInfo)
        {
            var fluentSimpleMemberMap = new FluentSimpleMemberMap();
            fluentSimpleMemberMap.Instance.Key = memberInfo.Name;
            fluentSimpleMemberMap.Instance.MemberName = memberInfo.Name;
            fluentSimpleMemberMap.Instance.MemberType = LateBoundReflection.GetMemberValueType(memberInfo);
            fluentSimpleMemberMap.Instance.MemberGetter = LateBoundReflection.GetGetter(memberInfo);
            fluentSimpleMemberMap.Instance.MemberSetter = LateBoundReflection.GetSetter(memberInfo);

            this.Instance.AddSimpleMemberMap(fluentSimpleMemberMap.Instance);
            return fluentSimpleMemberMap;
        }

        public FluentSimpleMemberMap Map(Expression<Func<TEntity, object>> member)
        {
            var memberInfo = this.GetSingleMember(member);
            return this.Map(memberInfo);
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
