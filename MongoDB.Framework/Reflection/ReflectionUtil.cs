using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MongoDB.Framework.Linq.Visitors;
using System.Linq.Expressions;

namespace MongoDB.Framework.Reflection
{
    public static class ReflectionUtil
    {
        public static Type GetMemberValueType(MemberInfo memberInfo)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).PropertyType;
            }

            throw new NotSupportedException("Only fields and properties are supported.");
        }

        public static MemberInfo GetSingleMember<TEntity>(string memberName)
        {
            var members = typeof(TEntity).GetMember(memberName);
            if (members.Length > 1)
                throw new InvalidOperationException(string.Format("More than one member found with memberName {0}.", memberName));

            return members[0];
        }

        public static MemberInfo GetSingleMember<TEntity, TMember>(Expression<Func<TEntity, TMember>> member)
        {
            var members = MemberInfoPathBuilder.BuildFrom(member);
            if (members.Count > 1)
                throw new InvalidOperationException("Only top level members are supported.");

            return members[0];
        }
    }
}