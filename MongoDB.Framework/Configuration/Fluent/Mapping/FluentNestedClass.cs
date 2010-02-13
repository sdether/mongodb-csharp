using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Reflection;
using System.Linq.Expressions;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    public class FluentNestedClass<TNestedClass> : FluentSuperClass<NestedClassMapModel, TNestedClass>
    {
        public FluentNestedClass()
            : base(new NestedClassMapModel(typeof(TNestedClass)))
        { }

        public void Parent(string memberName)
        {
            var memberInfo = ReflectionUtil.GetSingleMember<TNestedClass>(memberName);
            this.Parent(memberInfo);
        }

        public void Parent(MemberInfo memberInfo)
        {
            var memberType = ReflectionUtil.GetMemberValueType(memberInfo);
            var parentMap = new ParentMemberMapModel()
            {
                Getter = memberInfo,
                Setter = memberInfo
            };

            this.Model.ParentMap = parentMap;
        }

        public void Parent(Expression<Func<TNestedClass, object>> member)
        {
            var memberInfo = ReflectionUtil.GetSingleMember(member);
            this.Parent(memberInfo);
        }
    }
}