using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Configuration.Mapping.Conventions;

namespace MongoDB.Framework.Configuration.Fluent.Mapping.Conventions
{
    public class FluentMemberFinder
    {
        private MappingConventions conventions;

        public FluentMemberFinder(MappingConventions conventions)
        {
            this.conventions = conventions;
        }

        public FluentMemberFinder AllPublicProperties()
        {
            conventions.MemberFinder = new CustomMemberFinder(t => true, MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public);
            return this;
        }

        public FluentMemberFinder AllDeclaredPublicProperties()
        {
            conventions.MemberFinder = new CustomMemberFinder(t => true, MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            return this;
        }

        public FluentMemberFinder AllPublicPropertiesAndFields()
        {
            conventions.MemberFinder = new CustomMemberFinder(t => true, MemberTypes.Property | MemberTypes.Field, BindingFlags.Instance | BindingFlags.Public);
            return this;
        }

        public FluentMemberFinder AllDeclaredPublicPropertiesAndFields()
        {
            conventions.MemberFinder = new CustomMemberFinder(t => true, MemberTypes.Property | MemberTypes.Field, BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            return this;
        }

        public FluentMemberFinder MembersMatching(MemberTypes memberTypes, BindingFlags bindingFlags)
        {
            conventions.MemberFinder = new CustomMemberFinder(t => true, memberTypes, bindingFlags);
            return this;
        }

        public FluentMemberFinder ExceptFor(Func<MemberInfo, bool> filter)
        {
            var filterMemberFinder = new FilteredMemberFinder(this.conventions.MemberFinder, m => filter(m));
            return this;
        }

    }
}