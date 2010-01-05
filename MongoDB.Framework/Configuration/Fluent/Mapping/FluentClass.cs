using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Configuration.Mapping.Models;
using MongoDB.Framework.Linq.Visitors;
using MongoDB.Framework.Mapping.Types;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    public abstract class FluentClass<TModel, TEntity> : FluentBase<TModel> where TModel : ClassMapModel
    {
        public FluentClass(TModel model)
            : base(model)
        { }

        public FluentCollection Collection(string memberName)
        {
            var memberInfo = ReflectionUtil.GetSingleMember<TEntity>(memberName);
            return this.Collection(memberInfo);
        }

        public FluentCollection Collection(MemberInfo memberInfo)
        {
            var memberType = ReflectionUtil.GetMemberValueType(memberInfo);
            var collection = new FluentCollection();
            collection.Model.Getter = memberInfo;
            collection.Model.Setter = memberInfo;

            this.Model.CollectionMaps.Add(collection.Model);
            return collection;
        }

        public FluentCollection Collection(Expression<Func<TEntity, object>> member)
        {
            var memberInfo = ReflectionUtil.GetSingleMember(member);
            return this.Collection(memberInfo);
        }

        public FluentValue Map(string memberName)
        {
            var memberInfo = ReflectionUtil.GetSingleMember<TEntity>(memberName);
            return this.Map(memberInfo);
        }

        public FluentValue Map(MemberInfo memberInfo)
        {
            var memberType = ReflectionUtil.GetMemberValueType(memberInfo);
            var value = new FluentValue();
            value.Model.Getter = memberInfo;
            value.Model.Setter = memberInfo;

            this.Model.ValueMaps.Add(value.Model);
            return value;
        }

        public FluentValue Map(Expression<Func<TEntity, object>> member)
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