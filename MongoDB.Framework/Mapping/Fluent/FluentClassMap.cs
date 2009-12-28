using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Linq.Visitors;
using MongoDB.Framework.Reflection;
using MongoDB.Framework.Mapping.Model;
using MongoDB.Framework.Mapping.Types;

namespace MongoDB.Framework.Mapping.Fluent
{
    public abstract class FluentClassMap<TModel, TEntity> : FluentMap<TModel> where TModel : ClassMapModel
    {
        public FluentClassMap(TModel model)
            : base(model)
        { }

        public FluentKeyValueMemberMap Map(string memberName)
        {
            var memberInfo = this.GetSingleMember(memberName);
            return this.Map(memberInfo);
        }

        public FluentKeyValueMemberMap Map(MemberInfo memberInfo)
        {
            var memberType = LateBoundReflection.GetMemberValueType(memberInfo);
            var memberMap = new FluentKeyValueMemberMap();
            memberMap.Model.Getter = memberInfo;
            memberMap.Model.Setter = memberInfo;

            this.Model.MemberMaps.Add(memberMap.Model);
            return memberMap;
        }

        public FluentKeyValueMemberMap Map(Expression<Func<TEntity, object>> member)
        {
            var memberInfo = this.GetSingleMember(member);
            return this.Map(memberInfo);
        }

        public FluentNestedClassMemberMapModel NestedClass<TNestedClass>(string memberName, Action<FluentNestedClassMap<TNestedClass>> configure)
        {
            var memberInfo = this.GetSingleMember(memberName);
            return this.NestedClass<TNestedClass>(memberInfo, configure);
        }

        public FluentNestedClassMemberMapModel NestedClass<TNestedClass>(MemberInfo memberInfo, Action<FluentNestedClassMap<TNestedClass>> configure)
        {
            var memberMap = new FluentNestedClassMemberMapModel();
            memberMap.Model.Getter = memberInfo;
            memberMap.Model.Setter = memberInfo;

            var nestedClassMap = new FluentNestedClassMap<TNestedClass>();
            configure(nestedClassMap);
            memberMap.Model.NestedClassMapModel = nestedClassMap.Model;
            this.Model.MemberMaps.Add(memberMap.Model);
            return memberMap;
        }

        public FluentNestedClassMemberMapModel NestedClass<TNestedClass>(Expression<Func<TEntity, TNestedClass>> member, Action<FluentNestedClassMap<TNestedClass>> configure)
        {
            var memberInfo = this.GetSingleMember(member);
            return this.NestedClass<TNestedClass>(memberInfo, configure);
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
