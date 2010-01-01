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
    public abstract class FluentClassMap<TModel, TEntity> : FluentBase<TModel> where TModel : ClassMapModel
    {
        public FluentClassMap(TModel model)
            : base(model)
        { }

        public FluentEmbeddedMemberMap<TEntity> Map(string memberName)
        {
            var memberInfo = ReflectionUtil.GetSingleMember<TEntity>(memberName);
            return this.Map(memberInfo);
        }

        public FluentEmbeddedMemberMap<TEntity> Map(MemberInfo memberInfo)
        {
            var memberType = ReflectionUtil.GetMemberValueType(memberInfo);
            var memberMap = new FluentEmbeddedMemberMap<TEntity>();
            memberMap.Model.Getter = memberInfo;
            memberMap.Model.Setter = memberInfo;

            this.Model.MemberMaps.Add(memberMap.Model);
            return memberMap;
        }

        public FluentEmbeddedMemberMap<TEntity> Map(Expression<Func<TEntity, object>> member)
        {
            var memberInfo = ReflectionUtil.GetSingleMember(member);
            return this.Map(memberInfo);
        }
    }
}