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
    public abstract class FluentDocumentMap<TMap, TEntity> : FluentMap<TMap> where TMap : DocumentMap
    {
        public void Component<TComponent>(string memberName, Action<FluentRootDocumentMap<TComponent>> configure)
        {
            var memberInfo = this.GetSingleMember(memberName);
            this.Component(memberInfo, configure);
        }

        public void Component<TComponent>(string memberName, string key, Action<FluentRootDocumentMap<TComponent>> configure)
        {
            var memberInfo = this.GetSingleMember(memberName);
            this.Component(memberInfo, key, configure);
        }

        public void Component<TComponent>(MemberInfo memberInfo, Action<FluentRootDocumentMap<TComponent>> configure)
        {
            this.Component(memberInfo, memberInfo.Name, configure);
        }

        public void Component<TComponent>(MemberInfo memberInfo, string key, Action<FluentRootDocumentMap<TComponent>> configure)
        {
            var fluentRootDocumentMap = new FluentRootDocumentMap<TComponent>();
            configure(fluentRootDocumentMap);
            var nestedDocumentValueMap = new NestedDocumentValueMap(
                key,
                memberInfo.Name,
                LateBoundReflection.GetMemberValueType(memberInfo),
                LateBoundReflection.GetGetter(memberInfo),
                LateBoundReflection.GetSetter(memberInfo),
                fluentRootDocumentMap.Instance);

            this.Instance.AddNestedDocumentValueMap(nestedDocumentValueMap);
        }

        public void Component<TComponent>(Expression<Func<TEntity, TComponent>> idMember, Action<FluentRootDocumentMap<TComponent>> configure)
        {
            var memberInfo = this.GetSingleMember(idMember);
            this.Component(memberInfo, configure);
        }

        public void Component<TComponent>(Expression<Func<TEntity, TComponent>> member, string key, Action<FluentRootDocumentMap<TComponent>> configure)
        {
            var memberInfo = this.GetSingleMember(member);
            this.Component<TComponent>(memberInfo, key, configure);
        }

        public void Map(string memberName)
        {
            var memberInfo = this.GetSingleMember(memberName);
            this.Map(memberInfo);
        }

        public void Map(string memberName, string key)
        {
            var memberInfo = this.GetSingleMember(memberName);
            this.Map(memberInfo, key);
        }

        public void Map(MemberInfo memberInfo)
        {
            this.Map(memberInfo, memberInfo.Name);
        }

        public void Map(MemberInfo memberInfo, string key)
        {
            var simpleValueMap = new SimpleValueMap(
                key,
                memberInfo.Name,
                LateBoundReflection.GetMemberValueType(memberInfo),
                LateBoundReflection.GetGetter(memberInfo),
                LateBoundReflection.GetSetter(memberInfo));

            this.Instance.AddSimpleValueMap(simpleValueMap);
        }

        public void Map(Expression<Func<TEntity, object>> member)
        {
            var memberInfo = this.GetSingleMember(member);
            this.Map(memberInfo);
        }

        public void Map(Expression<Func<TEntity, object>> member, string key)
        {
            var memberInfo = this.GetSingleMember(member);
            this.Map(memberInfo, key);
        }

        public void References(string memberName)
        {
            var memberInfo = this.GetSingleMember(memberName);
            this.References(memberInfo);
        }

        public void References(string memberName, string key)
        {
            var memberInfo = this.GetSingleMember(memberName);
            this.References(memberInfo, key);
        }

        public void References(MemberInfo memberInfo)
        {
            this.References(memberInfo, memberInfo.Name);
        }

        public void References(MemberInfo memberInfo, string key)
        {
            var referenceValueMap = new ReferenceValueMap(
                key,
                memberInfo.Name,
                LateBoundReflection.GetMemberValueType(memberInfo),
                LateBoundReflection.GetGetter(memberInfo),
                LateBoundReflection.GetSetter(memberInfo));

            this.Instance.AddReferenceValueMap(referenceValueMap);
        }

        public void References(Expression<Func<TEntity, object>> member)
        {
            var memberInfo = this.GetSingleMember(member);
            this.References(memberInfo);
        }

        public void References(Expression<Func<TEntity, object>> member, string key)
        {
            var memberInfo = this.GetSingleMember(member);
            this.References(memberInfo, key);
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
