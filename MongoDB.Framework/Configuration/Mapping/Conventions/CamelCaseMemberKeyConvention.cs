using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public class CamelCaseMemberKeyConvention : ConventionBase<MemberInfo>, IMemberKeyConvention
    {
        public static readonly CamelCaseMemberKeyConvention AlwaysMatching = new CamelCaseMemberKeyConvention(m => true);

        public CamelCaseMemberKeyConvention(Func<MemberInfo, bool> matcher)
            : base(matcher)
        { }

        public string GetKey(MemberInfo member)
        {
            return Inflector.ToCamelCase(member.Name);
        }
    }
}