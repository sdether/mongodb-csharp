using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public class DefaultMemberFinder : ConventionBase<Type>, IMemberFinder
    {
        public static readonly DefaultMemberFinder AlwaysMatching = new DefaultMemberFinder();

        private DefaultMemberFinder()
            : base(t => true)
        { }

        public IEnumerable<MemberInfo> FindMembers(Type type)
        {
            return Enumerable.Empty<MemberInfo>();
        }
    }
}