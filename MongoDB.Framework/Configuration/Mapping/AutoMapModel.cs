using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Configuration.Mapping.Conventions;

namespace MongoDB.Framework.Configuration.Mapping
{
    public class AutoMapModel
    {
        #region Static

        private readonly static Stack<ICollectionConvention> globalCollectionConventions = new Stack<ICollectionConvention>();
        private readonly static Stack<ICollectionNameConvention> globalCollectionNameConventions = new Stack<ICollectionNameConvention>();
        private readonly static Stack<IDiscriminatorConvention> globalDiscriminatorConventions = new Stack<IDiscriminatorConvention>();
        private readonly static Stack<IExtendedPropertiesConvention> globalExtendedPropertiesConventions = new Stack<IExtendedPropertiesConvention>();
        private readonly static Stack<IIdConvention> globalIdConventions = new Stack<IIdConvention>();
        private readonly static Stack<IMemberKeyConvention> globalMemberKeyConventions = new Stack<IMemberKeyConvention>();
        private readonly static Stack<IValueConverterConvention> globalValueConverterConventions = new Stack<IValueConverterConvention>();

        static AutoMapModel()
        {
            AddCollectionConvention(DefaultCollectionConvention.AlwaysMatching);
            AddCollectionNameConvention(TypeNameCollectionNameConvention.AlwaysMatching);
            AddDiscriminatorConvention(NullDiscriminatorConvention.AlwaysMatching);
            AddExtendedPropertiesConvention(NullExtendedPropertiesConvention.AlwaysMatching);
            AddIdConvention(NullIdConvention.AlwaysMatching);
            AddMemberKeyConvention(MemberNameMemberKeyConvention.AlwaysMatching);
            AddValueConverterConvention(DefaultValueConverterConvention.AlwaysMatching);
        }

        public static void AddCollectionConvention(ICollectionConvention convention)
        {
            if (convention == null)
                throw new ArgumentNullException("convention");

            globalCollectionConventions.Push(convention);
        }

        public static void AddCollectionNameConvention(ICollectionNameConvention convention)
        {
            if (convention == null)
                throw new ArgumentNullException("convention");

            globalCollectionNameConventions.Push(convention);
        }

        public static void AddDiscriminatorConvention(IDiscriminatorConvention convention)
        {
            if (convention == null)
                throw new ArgumentNullException("convention");

            globalDiscriminatorConventions.Push(convention);
        }

        public static void AddExtendedPropertiesConvention(IExtendedPropertiesConvention convention)
        {
            if (convention == null)
                throw new ArgumentNullException("convention");

            globalExtendedPropertiesConventions.Push(convention);
        }

        public static void AddIdConvention(IIdConvention convention)
        {
            if (convention == null)
                throw new ArgumentNullException("convention");

            globalIdConventions.Push(convention);
        }

        public static void AddMemberKeyConvention(IMemberKeyConvention convention)
        {
            if (convention == null)
                throw new ArgumentNullException("convention");

            globalMemberKeyConventions.Push(convention);
        }

        public static void AddValueConverterConvention(IValueConverterConvention convention)
        {
            if (convention == null)
                throw new ArgumentNullException("convention");

            globalValueConverterConventions.Push(convention);
        }

        #endregion

        public ICollectionConvention CollectionConvention { get; set; }

        public ICollectionNameConvention CollectionNameConvention { get; set; }

        public IDiscriminatorConvention DiscriminatorConvention { get; set; }

        public IExtendedPropertiesConvention ExtendedPropertiesConvention { get; set; }

        public IIdConvention IdConvention { get; set; }

        public IMemberKeyConvention MemberKeyConvention { get; set; }

        public IValueConverterConvention ValueConverterConvention { get; set; }

        public ICollectionConvention GetCollectionConvention(Type type)
        {
            if (this.CollectionConvention != null && this.CollectionConvention.Matches(type))
                return this.CollectionConvention;

            return globalCollectionConventions.First(c => c.Matches(type));
        }

        public ICollectionNameConvention GetCollectionNameConvention(Type type)
        {
            if (this.CollectionNameConvention != null && this.CollectionNameConvention.Matches(type))
                return this.CollectionNameConvention;

            return globalCollectionNameConventions.First(c => c.Matches(type));
        }

        public IDiscriminatorConvention GetDiscriminatorConvention(Type type)
        {
            if (this.DiscriminatorConvention != null && this.DiscriminatorConvention.Matches(type))
                return this.DiscriminatorConvention;

            return globalDiscriminatorConventions.First(c => c.Matches(type));
        }

        public IExtendedPropertiesConvention GetExtendedPropertiesConvention(Type type)
        {
            if (this.ExtendedPropertiesConvention != null && this.ExtendedPropertiesConvention.Matches(type))
                return this.ExtendedPropertiesConvention;

            return globalExtendedPropertiesConventions.First(c => c.Matches(type));
        }

        public IIdConvention GetIdConvention(Type type)
        {
            if (this.IdConvention != null && this.IdConvention.Matches(type))
                return this.IdConvention;

            return globalIdConventions.First(c => c.Matches(type));
        }

        public IMemberKeyConvention GetMemberKeyConvention(MemberInfo member)
        {
            if (this.MemberKeyConvention != null && this.MemberKeyConvention.Matches(member))
                return this.MemberKeyConvention;

            return globalMemberKeyConventions.First(c => c.Matches(member));
        }

        public IValueConverterConvention GetValueConverterConvention(MemberInfo member)
        {
            if (this.ValueConverterConvention != null && this.ValueConverterConvention.Matches(member))
                return this.ValueConverterConvention;

            return globalValueConverterConventions.First(c => c.Matches(member));
        }
    }
}