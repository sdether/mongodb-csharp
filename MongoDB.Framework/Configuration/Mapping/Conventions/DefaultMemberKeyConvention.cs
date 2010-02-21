using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public class DefaultMemberKeyConvention : ConventionBase<MemberInfo>, IMemberKeyConvention
    {
        public static readonly DefaultMemberKeyConvention AlwaysMatching = new DefaultMemberKeyConvention(m => true);

        public DefaultMemberKeyConvention(Func<MemberInfo, bool> matcher)
            : base(matcher)
        { }

        public string GetKey(MemberInfo member)
        {
            return member.Name;
        }
    }
}