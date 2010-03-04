using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Mapping.Auto
{
    public class DefaultMemberFinder : IMemberFinder
    {
        public static readonly DefaultMemberFinder Instance = new DefaultMemberFinder();

        private DefaultMemberFinder()
        { }

        public IEnumerable<MemberInfo> FindMembers(Type type)
        {
            return Enumerable.Empty<MemberInfo>();            
        }
    }
}
