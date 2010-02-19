using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public class MemberNameMemberKeyConvention : ConventionBase<MemberInfo>, IMemberKeyConvention
    {
        public static readonly MemberNameMemberKeyConvention AlwaysMatching = new MemberNameMemberKeyConvention(m => true);

        public MemberNameMemberKeyConvention(Func<MemberInfo, bool> matcher)
            : base(matcher)
        { }

        public string GetKey(MemberInfo member)
        {
            return member.Name;
        }
    }
}