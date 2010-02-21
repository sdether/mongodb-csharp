using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Configuration.Fluent.Mapping.Conventions;
using MongoDB.Framework.Configuration.Mapping;
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

        public FluentMember Map(string memberName)
        {
            var memberInfo = ReflectionUtil.GetSingleMember<TEntity>(memberName);
            return this.Map(memberInfo);
        }

        public FluentMember Map(MemberInfo memberInfo)
        {
            var memberType = ReflectionUtil.GetMemberValueType(memberInfo);
            var value = new FluentMember(this.Model);
            value.Model.Getter = memberInfo;
            value.Model.Setter = memberInfo;

            this.Model.PersistentMemberMaps.Add(value.Model);
            return value;
        }

        public FluentMember Map(Expression<Func<TEntity, object>> member)
        {
            var memberInfo = ReflectionUtil.GetSingleMember(member);
            return this.Map(memberInfo);
        }
    }
}