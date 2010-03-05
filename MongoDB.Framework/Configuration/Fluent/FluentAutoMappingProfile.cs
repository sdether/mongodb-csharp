using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping.Auto;
using MongoDB.Framework.Mapping.Conventions;
using System.Reflection;

namespace MongoDB.Framework.Configuration.Fluent
{
    public class FluentAutoMappingProfile
    {
        private AutoMappingProfile setup;

        public FluentAutoMappingProfile(AutoMappingProfile setup)
        {
            this.setup = setup;
        }

        public void CollectionsAreNamed(Func<Type, string> name)
        {
            this.setup.Conventions.CollectionNameConvention = new DelegateCollectionNameConvention(name);
        }

        public void CollectionNamesAreCamelCasedAndPlural()
        {
            this.setup.Conventions.CollectionNameConvention = new DelegateCollectionNameConvention(x => Inflector.MakePlural(Inflector.ToCamelCase(x.Name)));
        }

        public void ExtendedPropertiesAreNamed(string name)
        {
            this.setup.Conventions.ExtendedPropertiesConvention = new DelegateExtendedPropertiesConvention(x => x.Name == name);
        }

        public void MemberKeysAreNamed(Func<MemberInfo, string> key)
        {
            this.setup.Conventions.MemberKeyConvention = new DelegateMemberKeyConvention(key);
        }

        public void MemberKeysAreCamelCased()
        {
            this.setup.Conventions.MemberKeyConvention = new DelegateMemberKeyConvention(x => Inflector.ToCamelCase(x.Name));
        }

        public void IdsAreNamed(string name)
        {
            this.setup.Conventions.IdConvention = new DelegateIdConvention(x => x.Name == name);
        }

        public void SubClassesAre(Func<Type, bool> isSubClass)
        {
            this.setup.IsSubClass = isSubClass;
        }
        
    }
}