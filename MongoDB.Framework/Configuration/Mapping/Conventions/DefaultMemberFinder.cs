using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public class DefaultMemberFinder : ConventionBase<Type>, IMemberFinder
    {
        public static readonly DefaultMemberFinder AlwaysMatching = new DefaultMemberFinder(t => true);

        private BindingFlags bindingFlags;
        private MemberTypes memberTypes;

        public DefaultMemberFinder(Func<Type, bool> matcher)
            : this(matcher, MemberTypes.Field | MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public)
        { }

        public DefaultMemberFinder(Func<Type, bool> matcher, MemberTypes memberTypes, BindingFlags bindingFlags)
            : base(matcher)
        {
            this.bindingFlags = bindingFlags;
            this.memberTypes = memberTypes;
        }

        public IEnumerable<MemberInfo> FindMembers(Type type)
        {
            return type.GetMembers(this.bindingFlags).Where(m => (this.memberTypes & m.MemberType) == m.MemberType);
        }
    }
}