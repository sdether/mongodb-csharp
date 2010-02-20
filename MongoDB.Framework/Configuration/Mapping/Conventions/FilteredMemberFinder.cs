using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public class FilteredMemberFinder : ConventionBase<Type>, IMemberFinder
    {
        private Func<MemberInfo, bool> filter;
        private IMemberFinder wrappedMemberFinder;

        public FilteredMemberFinder(IMemberFinder wrappedMemberFinder, Func<MemberInfo, bool> filter)
            : base(wrappedMemberFinder.Matches)
        {
            if (wrappedMemberFinder == null)
                throw new ArgumentNullException("wrappedMemberFinder");
            if (filter == null)
                throw new ArgumentNullException("filter");

            this.filter = filter;
            this.wrappedMemberFinder = wrappedMemberFinder;
        }

        public IEnumerable<System.Reflection.MemberInfo> FindMembers(Type type)
        {
            return this.wrappedMemberFinder.FindMembers(type).Where(m => filter(m));
        }
    }
}
