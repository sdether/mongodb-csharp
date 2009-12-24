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
        public FluentComponentValueMap<TComponent> Component<TComponent>(string memberName)
        {
            var memberInfo = this.GetSingleMember(memberName);
            return this.Component<TComponent>(memberInfo);
        }

        public FluentComponentValueMap<TComponent> Component<TComponent>(MemberInfo memberInfo)
        {
            var fluentComponentClassMap = new FluentComponentClassMap<TComponent>();
            var fluentComponentValueMap = new FluentComponentValueMap<TComponent>(fluentComponentClassMap);
            fluentComponentValueMap.Instance.Key = memberInfo.Name;
            fluentComponentValueMap.Instance.MemberName = memberInfo.Name;
            fluentComponentValueMap.Instance.MemberType = LateBoundReflection.GetMemberValueType(memberInfo);
            fluentComponentValueMap.Instance.MemberGetter = LateBoundReflection.GetGetter(memberInfo);
            fluentComponentValueMap.Instance.MemberSetter = LateBoundReflection.GetSetter(memberInfo);
            this.Instance.AddComponentValueMap(fluentComponentValueMap.Instance);
            return fluentComponentValueMap;
        }

        public FluentComponentValueMap<TComponent> Component<TComponent>(Expression<Func<TEntity, TComponent>> member)
        {
            var memberInfo = this.GetSingleMember(member);
            return this.Component<TComponent>(memberInfo);
        }

        public FluentSimpleValueMap Map(string memberName)
        {
            var memberInfo = this.GetSingleMember(memberName);
            return this.Map(memberInfo);
        }

        public FluentSimpleValueMap Map(MemberInfo memberInfo)
        {
            var fluentSimpleValueMap = new FluentSimpleValueMap();
            fluentSimpleValueMap.Instance.Key = memberInfo.Name;
            fluentSimpleValueMap.Instance.MemberName = memberInfo.Name;
            fluentSimpleValueMap.Instance.MemberType = LateBoundReflection.GetMemberValueType(memberInfo);
            fluentSimpleValueMap.Instance.MemberGetter = LateBoundReflection.GetGetter(memberInfo);
            fluentSimpleValueMap.Instance.MemberSetter = LateBoundReflection.GetSetter(memberInfo);

            this.Instance.AddSimpleValueMap(fluentSimpleValueMap.Instance);
            return fluentSimpleValueMap;
        }

        public FluentSimpleValueMap Map(Expression<Func<TEntity, object>> member)
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
