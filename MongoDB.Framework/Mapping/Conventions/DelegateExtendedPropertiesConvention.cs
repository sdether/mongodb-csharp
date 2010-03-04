using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Mapping.Conventions
{
    public class DelegateExtendedPropertiesConvention : IExtendedPropertiesConvention
    {
        private Func<MemberInfo, bool> isExtendedProperties;

        public DelegateExtendedPropertiesConvention(Func<MemberInfo, bool> isExtendedProperties)
        {
            if (isExtendedProperties == null)
                throw new ArgumentNullException("isExtendedProperties");

            this.isExtendedProperties = isExtendedProperties;
        }

        public bool IsExtendedProperties(MemberInfo memberInfo)
        {
            return isExtendedProperties(memberInfo);
        }
    }
}
