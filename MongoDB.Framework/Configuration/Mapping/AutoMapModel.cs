using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Configuration.Mapping.Conventions;
using MongoDB.Framework.Mapping;

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

        #region Public Properties

        public ICollectionConvention CollectionConvention { get; set; }

        public ICollectionNameConvention CollectionNameConvention { get; set; }

        public IDiscriminatorConvention DiscriminatorConvention { get; set; }

        public IExtendedPropertiesConvention ExtendedPropertiesConvention { get; set; }

        public IIdConvention IdConvention { get; set; }

        public IMemberKeyConvention MemberKeyConvention { get; set; }

        public IValueConverterConvention ValueConverterConvention { get; set; }

        #endregion

        #region Public Methods

        public ICollectionConvention GetCollectionConvention(Type type)
        {
            var conventions = new List<ICollectionConvention>();
            if (this.CollectionConvention != null && this.CollectionConvention.Matches(type))
                conventions.Add(this.CollectionConvention);

            conventions.AddRange(globalCollectionConventions.Where(c => c.Matches(type)));

            return new CompositeCollectionConvention(conventions);
        }

        public ICollectionNameConvention GetCollectionNameConvention(Type type)
        {
            if (this.CollectionNameConvention != null && this.CollectionNameConvention.Matches(type))
                return this.CollectionNameConvention;

            return globalCollectionNameConventions.First(c => c.Matches(type));
        }

        public IDiscriminatorConvention GetDiscriminatorConvention(Type type)
        {
            var conventions = new List<IDiscriminatorConvention>();
            if (this.DiscriminatorConvention != null && this.DiscriminatorConvention.Matches(type))
                conventions.Add(this.DiscriminatorConvention);

            conventions.AddRange(globalDiscriminatorConventions.Where(c => c.Matches(type)));

            return new CompositeDiscriminatorConvention(conventions);
        }

        public IExtendedPropertiesConvention GetExtendedPropertiesConvention(Type type)
        {
            var conventions = new List<IExtendedPropertiesConvention>();
            if (this.ExtendedPropertiesConvention != null && this.ExtendedPropertiesConvention.Matches(type))
                conventions.Add(this.ExtendedPropertiesConvention);

            conventions.AddRange(globalExtendedPropertiesConventions.Where(c => c.Matches(type)));

            return new CompositeExtendedPropertiesConvention(conventions);
        }

        public IIdConvention GetIdConvention(Type type)
        {
            var conventions = new List<IIdConvention>();
            if (this.IdConvention != null && this.IdConvention.Matches(type))
                conventions.Add(this.IdConvention);

            conventions.AddRange(globalIdConventions.Where(c => c.Matches(type)));

            return new CompositeIdConvention(conventions);
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

        #endregion

        #region Private Nested Classes

        private abstract class CompositeConvention<TCollection, TConvention> : IConvention<TConvention> where TCollection : IConvention<TConvention>
        {
            protected readonly IEnumerable<TCollection> conventions;

            public CompositeConvention(IEnumerable<TCollection> conventions)
            {
                this.conventions = conventions;
            }

            public bool Matches(TConvention topic)
            {
                return true;
            }
        }

        private class CompositeCollectionConvention : CompositeConvention<ICollectionConvention, Type>, ICollectionConvention
        {
            public CompositeCollectionConvention(IEnumerable<ICollectionConvention> conventions)
                : base(conventions)
            { }

            public ICollectionType GetCollectionType(Type type)
            {
                return this.conventions.First(c => c.IsCollection(type)).GetCollectionType(type);
            }

            public Type GetElementType(Type type)
            {
                return this.conventions.First(c => c.IsCollection(type)).GetElementType(type);
            }

            public bool IsCollection(Type type)
            {
                return this.conventions.Any(c => c.IsCollection(type));
            }
        }

        private class CompositeDiscriminatorConvention : CompositeConvention<IDiscriminatorConvention, Type>, IDiscriminatorConvention
        {
            public CompositeDiscriminatorConvention(IEnumerable<IDiscriminatorConvention> conventions)
                : base(conventions)
            { }

            public string GetDiscriminatorKey(Type type)
            {
                return this.conventions.First(c => c.HasDiscriminator(type)).GetDiscriminatorKey(type);
            }

            public object GetDiscriminator(Type type)
            {
                return this.conventions.First(c => c.HasDiscriminator(type)).GetDiscriminator(type);
            }

            public bool HasDiscriminator(Type type)
            {
                return this.conventions.Any(c => c.HasDiscriminator(type));
            }
        }

        private class CompositeExtendedPropertiesConvention : CompositeConvention<IExtendedPropertiesConvention, Type>, IExtendedPropertiesConvention
        {
            public CompositeExtendedPropertiesConvention(IEnumerable<IExtendedPropertiesConvention> conventions)
                : base(conventions)
            { }

            public ExtendedPropertiesMapModel GetExtendedPropertiesMapModel(Type type)
            {
                return this.conventions.First(c => c.HasExtendedProperties(type)).GetExtendedPropertiesMapModel(type);
            }

            public bool HasExtendedProperties(Type type)
            {
                return this.conventions.Any(c => c.HasExtendedProperties(type));
            }
        }

        private class CompositeIdConvention : CompositeConvention<IIdConvention, Type>, IIdConvention
        {
            public CompositeIdConvention(IEnumerable<IIdConvention> conventions)
                : base(conventions)
            { }

            public IdMapModel GetIdMapModel(Type type)
            {
                return this.conventions.First(c => c.HasId(type)).GetIdMapModel(type);
            }

            public bool HasId(Type type)
            {
                return this.conventions.Any(c => c.HasId(type));
            }
        }

        #endregion
    }
}