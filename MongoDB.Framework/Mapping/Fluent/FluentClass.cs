using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Linq.Visitors;
using MongoDB.Framework.Reflection;
using MongoDB.Framework.Mapping.Models;
using MongoDB.Framework.Mapping.Types;

namespace MongoDB.Framework.Mapping.Fluent
{
    public abstract class FluentClass<TModel, TEntity> : FluentBase<TModel> where TModel : ClassMapModel
    {
        public FluentClass(TModel model)
            : base(model)
        { }

        public FluentEmbeddedMember<TEntity> Map(string memberName)
        {
            var memberInfo = ReflectionUtil.GetSingleMember<TEntity>(memberName);
            return this.Map(memberInfo);
        }

        public FluentEmbeddedMember<TEntity> Map(MemberInfo memberInfo)
        {
            var memberType = ReflectionUtil.GetMemberValueType(memberInfo);
            var memberMap = new FluentEmbeddedMember<TEntity>();
            memberMap.Model.Getter = memberInfo;
            memberMap.Model.Setter = memberInfo;

            this.Model.MemberMaps.Add(memberMap.Model);
            return memberMap;
        }

        public FluentEmbeddedMember<TEntity> Map(Expression<Func<TEntity, object>> member)
        {
            var memberInfo = ReflectionUtil.GetSingleMember(member);
            return this.Map(memberInfo);
        }

        public FluentReference References(string memberName)
        {
            var memberInfo = ReflectionUtil.GetSingleMember<TEntity>(memberName);
            return this.References(memberInfo);
        }

        public FluentReference References(MemberInfo memberInfo)
        {
            var memberType = ReflectionUtil.GetMemberValueType(memberInfo);
            var reference = new FluentReference();
            reference.Model.Getter = memberInfo;
            reference.Model.Setter = memberInfo;

            this.Model.ManyToOneMaps.Add(reference.Model);
            return reference;
        }

        public FluentReference References(Expression<Func<TEntity, object>> member)
        {
            var memberInfo = ReflectionUtil.GetSingleMember(member);
            return this.References(memberInfo);
        }
    }
}