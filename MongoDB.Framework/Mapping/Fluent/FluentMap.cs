using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping.Models;
using MongoDB.Framework.Reflection;
using System.Reflection;
using System.Linq.Expressions;
using MongoDB.Framework.Linq.Visitors;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentMap<TModel, TEntity> where TModel : ClassMapModel
    {
        private FluentClassMap<TModel, TEntity> owner;

        public FluentMap(FluentClassMap<TModel, TEntity> owner)
        {
            this.owner = owner;
        }

        public FluentEmbeddedValueMap One(string memberName)
        {
            var memberInfo = ReflectionUtil.GetSingleMember<TEntity>(memberName);
            return this.One(memberInfo);
        }

        public FluentEmbeddedValueMap One(MemberInfo memberInfo)
        {
            var memberType = ReflectionUtil.GetMemberValueType(memberInfo);
            var valueMap = new FluentEmbeddedValueMap();
            valueMap.Model.Getter = memberInfo;
            valueMap.Model.Setter = memberInfo;

            this.owner.Model.MemberMaps.Add(valueMap.Model);
            return valueMap;
        }

        public FluentEmbeddedValueMap One(Expression<Func<TEntity, object>> member)
        {
            var memberInfo = ReflectionUtil.GetSingleMember(member);
            return this.One(memberInfo);
        }

        public FluentEmbeddedCollectionMap Many(string memberName)
        {
            var memberInfo = ReflectionUtil.GetSingleMember<TEntity>(memberName);
            return this.Many(memberInfo);
        }

        public FluentEmbeddedCollectionMap Many(MemberInfo memberInfo)
        {
            var memberType = ReflectionUtil.GetMemberValueType(memberInfo);
            var collectionMap = new FluentEmbeddedCollectionMap();
            collectionMap.Model.Getter = memberInfo;
            collectionMap.Model.Setter = memberInfo;

            this.owner.Model.MemberMaps.Add(collectionMap.Model);
            return collectionMap;
        }

        public FluentEmbeddedCollectionMap Many(Expression<Func<TEntity, object>> member)
        {
            var memberInfo = ReflectionUtil.GetSingleMember(member);
            return this.Many(memberInfo);
        }
    }
}