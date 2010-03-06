using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Mapper.Mapping.Conventions
{
    public class DelegateIdConvention: IIdConvention
    {
        private Func<MemberInfo, bool> isId;

        public DelegateIdConvention(Func<MemberInfo, bool> isId)
        {
            if (isId == null)
                throw new ArgumentNullException("isId");

            this.isId = isId;
        }

        public bool IsId(MemberInfo memberInfo)
        {
            return isId(memberInfo);
        }
    }
}
