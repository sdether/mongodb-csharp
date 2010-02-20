using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public class NullMemberFinder : ConventionBase<Type>, IMemberFinder
    {
        public static readonly NullMemberFinder AlwaysMatching = new NullMemberFinder();

        private NullMemberFinder()
            : base(t => true)
        { }

        public IEnumerable<MemberInfo> FindMembers(Type type)
        {
            return Enumerable.Empty<MemberInfo>();
        }
    }
}