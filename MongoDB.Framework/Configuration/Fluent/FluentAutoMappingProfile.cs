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
        public AutoMappingProfile Profile { get; private set; }

        public FluentAutoMappingProfile(AutoMappingProfile profile)
        {
            this.Profile = profile;
        }

        public void CollectionsAreNamed(Func<Type, string> name)
        {
            this.Profile.Conventions.CollectionNameConvention = new DelegateCollectionNameConvention(name);
        }

        public void CollectionNamesAreCamelCasedAndPlural()
        {
            this.Profile.Conventions.CollectionNameConvention = new DelegateCollectionNameConvention(x => Inflector.MakePlural(Inflector.ToCamelCase(x.Name)));
        }

        public void DiscriminatorKeysAre(string key)
        {
            this.Profile.Conventions.DiscriminatorKeyConvention = new DelegateDiscriminatorKeyConvention(t => key);
        }

        public void ExtendedPropertiesAreNamed(string name)
        {
            this.Profile.Conventions.ExtendedPropertiesConvention = new DelegateExtendedPropertiesConvention(x => x.Name == name);
        }

        public void MemberKeysAreNamed(Func<MemberInfo, string> key)
        {
            this.Profile.Conventions.MemberKeyConvention = new DelegateMemberKeyConvention(key);
        }

        public void MemberKeysAreCamelCased()
        {
            this.Profile.Conventions.MemberKeyConvention = new DelegateMemberKeyConvention(x => Inflector.ToCamelCase(x.Name));
        }

        public void IdsAreNamed(string name)
        {
            this.Profile.Conventions.IdConvention = new DelegateIdConvention(x => x.Name == name);
        }

        public void SubClassesAre(Func<Type, bool> isSubClass)
        {
            this.Profile.IsSubClass = isSubClass;
        }
    }
}