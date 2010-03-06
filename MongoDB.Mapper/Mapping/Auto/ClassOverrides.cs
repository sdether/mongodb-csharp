using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Mapper.Mapping.Auto
{
    public class ClassOverrides
    {
        private Dictionary<MemberInfo, MemberOverrides> memberOverridesMap;

        public string CollectionName { get; set; }

        public ClassOverrides()
        {
            this.memberOverridesMap = new Dictionary<MemberInfo, MemberOverrides>();
        }

        public MemberOverrides GetOverridesFor(MemberInfo memberInfo)
        {
            MemberOverrides memberOverrides;
            if (!this.memberOverridesMap.TryGetValue(memberInfo, out memberOverrides))
                memberOverrides = this.memberOverridesMap[memberInfo] = new MemberOverrides();

            return memberOverrides;
        }
    }
}