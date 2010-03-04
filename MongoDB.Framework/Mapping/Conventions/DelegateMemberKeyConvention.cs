using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Mapping.Conventions
{
    public class DelegateMemberKeyConvention : IMemberKeyConvention
    {
        private Func<MemberInfo, string> memberKey;

        public DelegateMemberKeyConvention(Func<MemberInfo, string> memberKey)
        {
            if (memberKey == null)
                throw new ArgumentNullException("memberKey");

            this.memberKey = memberKey;
        }

        public string GetMemberKey(MemberInfo memberInfo)
        {
            return this.memberKey(memberInfo);
        }
    }
}