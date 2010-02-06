using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Linq.Visitors;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    public abstract class FluentClass<TModel, TEntity> : FluentBase<TModel> where TModel : ClassMapModel
    {
        public object Discriminator
        {
            get { return this.Model.Discriminator; }
            set { this.Model.Discriminator = value; }
        }

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

        public FluentMap Map(string memberName)
        {
            var memberInfo = ReflectionUtil.GetSingleMember<TEntity>(memberName);
            return this.Map(memberInfo);
        }

        public FluentMap Map(MemberInfo memberInfo)
        {
            var memberType = ReflectionUtil.GetMemberValueType(memberInfo);
            var value = new FluentMap();
            value.Model.Getter = memberInfo;
            value.Model.Setter = memberInfo;

            this.Model.ValueMaps.Add(value.Model);
            return value;
        }

        public FluentMap Map(Expression<Func<TEntity, object>> member)
        {
            var memberInfo = ReflectionUtil.GetSingleMember(member);
            return this.Map(memberInfo);
        }

        public FluentReference Reference(string memberName)
        {
            var memberInfo = ReflectionUtil.GetSingleMember<TEntity>(memberName);
            return this.Reference(memberInfo);
        }

        public FluentReference Reference(MemberInfo memberInfo)
        {
            var memberType = ReflectionUtil.GetMemberValueType(memberInfo);
            var reference = new FluentReference();
            reference.Model.Getter = memberInfo;
            reference.Model.Setter = memberInfo;

            this.Model.ManyToOneMaps.Add(reference.Model);
            return reference;
        }

        public FluentReference Reference(Expression<Func<TEntity, object>> member)
        {
            var memberInfo = ReflectionUtil.GetSingleMember(member);
            return this.Reference(memberInfo);
        }
    }
}