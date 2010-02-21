using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public class CustomMemberFinder : ConventionBase<Type>, IMemberFinder
    {
        public static readonly CustomMemberFinder AlwaysMatching = new CustomMemberFinder(t => true);

        private BindingFlags bindingFlags;
        private MemberTypes memberTypes;

        public CustomMemberFinder(Func<Type, bool> matcher)
            : this(matcher, MemberTypes.Field | MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public)
        { }

        public CustomMemberFinder(Func<Type, bool> matcher, MemberTypes memberTypes, BindingFlags bindingFlags)
            : base(matcher)
        {
            this.bindingFlags = bindingFlags;
            this.memberTypes = memberTypes;
        }

        public IEnumerable<MemberInfo> FindMembers(Type type)
        {
            return type
                .GetMembers(this.bindingFlags)
                .Where(m => (this.memberTypes & m.MemberType) == m.MemberType && ReflectionUtil.CanReadAndWrite(m));
        }
    }
}