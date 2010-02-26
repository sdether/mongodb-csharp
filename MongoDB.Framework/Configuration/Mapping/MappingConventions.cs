using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Configuration.Mapping.Conventions;
using MongoDB.Framework.Mapping;

namespace MongoDB.Framework.Configuration.Mapping
{
    public class MappingConventions
    {
        #region Static

        private readonly static Stack<IClassActivatorConvention> globalClassActivatorConventions = new Stack<IClassActivatorConvention>();
        private readonly static Stack<ICollectionConvention> globalCollectionConventions = new Stack<ICollectionConvention>();
        private readonly static Stack<ICollectionNameConvention> globalCollectionNameConventions = new Stack<ICollectionNameConvention>();
        private readonly static Stack<IExtendedPropertiesConvention> globalExtendedPropertiesConventions = new Stack<IExtendedPropertiesConvention>();
        private readonly static Stack<IIdConvention> globalIdConventions = new Stack<IIdConvention>();
        private readonly static Stack<IMemberFinder> globalMemberFinders = new Stack<IMemberFinder>();
        private readonly static Stack<IMemberKeyConvention> globalMemberKeyConventions = new Stack<IMemberKeyConvention>();
        private readonly static Stack<IValueConverterConvention> globalValueConverterConventions = new Stack<IValueConverterConvention>();

        static MappingConventions()
        {
            AddGlobalClassActivatorConvention(DefaultClassActivatorConvention.AlwaysMatching);
            AddGlobalCollectionConvention(DefaultCollectionConvention.AlwaysMatching);
            AddGlobalCollectionNameConvention(DefaultCollectionNameConvention.AlwaysMatching);
            AddGlobalExtendedPropertiesConvention(DefaultExtendedPropertiesConvention.AlwaysMatching);
            AddGlobalIdConvention(DefaultIdConvention.AlwaysMatching);
            AddGlobalMemberKeyConvention(DefaultMemberKeyConvention.AlwaysMatching);
            AddGlobalValueConverterConvention(DefaultValueConverterConvention.AlwaysMatching);
        }

        public static void AddGlobalClassActivatorConvention(IClassActivatorConvention convention)
        {
            if (convention == null)
                throw new ArgumentNullException("convention");

            globalClassActivatorConventions.Push(convention);
        }

        public static void AddGlobalCollectionConvention(ICollectionConvention convention)
        {
            if (convention == null)
                throw new ArgumentNullException("convention");

            globalCollectionConventions.Push(convention);
        }

        public static void AddGlobalCollectionNameConvention(ICollectionNameConvention convention)
        {
            if (convention == null)
                throw new ArgumentNullException("convention");

            globalCollectionNameConventions.Push(convention);
        }

        public static void AddGlobalExtendedPropertiesConvention(IExtendedPropertiesConvention convention)
        {
            if (convention == null)
                throw new ArgumentNullException("convention");

            globalExtendedPropertiesConventions.Push(convention);
        }

        public static void AddGlobalIdConvention(IIdConvention convention)
        {
            if (convention == null)
                throw new ArgumentNullException("convention");

            globalIdConventions.Push(convention);
        }

        public static void AddGlobalMemberFinder(IMemberFinder memberFinder)
        {
            if (memberFinder == null)
                throw new ArgumentNullException("memberFinder");

            globalMemberFinders.Push(memberFinder);
        }

        public static void AddGlobalMemberKeyConvention(IMemberKeyConvention convention)
        {
            if (convention == null)
                throw new ArgumentNullException("convention");

            globalMemberKeyConventions.Push(convention);
        }

        public static void AddGlobalValueConverterConvention(IValueConverterConvention convention)
        {
            if (convention == null)
                throw new ArgumentNullException("convention");

            globalValueConverterConventions.Push(convention);
        }

        #endregion

        #region Public Properties

        public IClassActivatorConvention ClassActivatorConvention { get; set; }

        public ICollectionConvention CollectionConvention { get; set; }

        public ICollectionNameConvention CollectionNameConvention { get; set; }

        public IExtendedPropertiesConvention ExtendedPropertiesConvention { get; set; }

        public IIdConvention IdConvention { get; set; }

        public IMemberFinder MemberFinder { get; set; }

        public IMemberKeyConvention MemberKeyConvention { get; set; }

        public IValueConverterConvention ValueConverterConvention { get; set; }

        #endregion

        #region Public Methods

        public MappingConventions Copy()
        {
            return new MappingConventions()
            {
                ClassActivatorConvention = this.ClassActivatorConvention,
                CollectionConvention = this.CollectionConvention,
                CollectionNameConvention = this.CollectionNameConvention,
                ExtendedPropertiesConvention = this.ExtendedPropertiesConvention,
                IdConvention = this.IdConvention,
                MemberFinder = this.MemberFinder,
                MemberKeyConvention = this.MemberKeyConvention,
                ValueConverterConvention = this.ValueConverterConvention
            };
        }

        public IClassActivatorConvention GetClassActivatorConvention(Type type)
        {
            var conventions = new List<IClassActivatorConvention>();
            if (this.ClassActivatorConvention != null && this.ClassActivatorConvention.Matches(type))
                conventions.Add(this.ClassActivatorConvention);

            conventions.AddRange(globalClassActivatorConventions.Where(c => c.Matches(type)));

            return new CompositeClassActivaterConvention(conventions);
        }

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

        public IMemberFinder GetMemberFinder(Type type)
        {
            var finders = new List<IMemberFinder>();
            if (this.MemberFinder != null && this.MemberFinder.Matches(type))
                finders.Add(this.MemberFinder);

            finders.AddRange(globalMemberFinders.Where(f => f.Matches(type)));

            return new CompositeMemberFinder(finders);
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

        private class CompositeClassActivaterConvention : CompositeConvention<IClassActivatorConvention, Type>, IClassActivatorConvention
        {
            public CompositeClassActivaterConvention(IEnumerable<IClassActivatorConvention> conventions)
                : base(conventions)
            { }

            public bool CanCreateActivator(Type type)
            {
                return this.conventions.Any(c => c.CanCreateActivator(type));
            }

            public IClassActivator CreateActivator(Type type)
            {
                return this.conventions.First(c => c.Matches(type)).CreateActivator(type);
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

        private class CompositeMemberFinder : CompositeConvention<IMemberFinder, Type>, IMemberFinder
        {
            public CompositeMemberFinder(IEnumerable<IMemberFinder> memberFinders)
                : base(memberFinders)
            { }

            public IEnumerable<MemberInfo> FindMembers(Type type)
            {
                return this.conventions.SelectMany(c => c.FindMembers(type));
            }
        }

        #endregion
    }
}