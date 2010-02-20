using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Configuration.Mapping.Conventions;
using System.Reflection;
using MongoDB.Framework.Configuration.Mapping;

namespace MongoDB.Framework.Configuration.Fluent.Mapping.Auto
{
    public class FluentAutoMemberFinder
    {
        private AutoMapModel autoMapModel;

        public FluentAutoMemberFinder(AutoMapModel autoMapModel)
        {
            this.autoMapModel = autoMapModel;
        }

        public FluentAutoMemberFinder AllPublicProperties()
        {
            autoMapModel.MemberFinder = new DefaultMemberFinder(t => true, MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public);
            return this;
        }

        public FluentAutoMemberFinder AllDeclaredPublicProperties()
        {
            autoMapModel.MemberFinder = new DefaultMemberFinder(t => true, MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            return this;
        }

        public FluentAutoMemberFinder AllPublicPropertiesAndFields()
        {
            autoMapModel.MemberFinder = new DefaultMemberFinder(t => true, MemberTypes.Property | MemberTypes.Field, BindingFlags.Instance | BindingFlags.Public);
            return this;
        }

        public FluentAutoMemberFinder AllDeclaredPublicPropertiesAndFields()
        {
            autoMapModel.MemberFinder = new DefaultMemberFinder(t => true, MemberTypes.Property | MemberTypes.Field, BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            return this;
        }

        public FluentAutoMemberFinder MembersMatching(MemberTypes memberTypes, BindingFlags bindingFlags)
        {
            autoMapModel.MemberFinder = new DefaultMemberFinder(t => true, memberTypes, bindingFlags);
            return this;
        }

        public FluentAutoMemberFinder ExceptFor(Func<MemberInfo, bool> filter)
        {
            var filterMemberFinder = new FilteredMemberFinder(this.autoMapModel.MemberFinder, m => filter(m));
            return this;
        }

    }
}