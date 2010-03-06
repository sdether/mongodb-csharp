using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Mapper.Mapping.Conventions;
using System.Reflection;
using MongoDB.Mapper.Reflection;

namespace MongoDB.Mapper.Mapping.Auto
{
    public class AutoMappingProfile
    {
        private ConventionProfile conventions;
        private Func<Type, bool> isSubClass;
        private IMemberFinder memberFinder;
        private Dictionary<Type, ClassOverrides> overrides;

        public ConventionProfile Conventions
        {
            get { return this.conventions; }
            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");

                this.conventions = value;
            }
        }

        public IMemberFinder MemberFinder
        {
            get { return this.memberFinder; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this.memberFinder = memberFinder;
            }
        }

        public Func<Type, bool> IsSubClass
        {
            get { return this.isSubClass; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this.isSubClass = value;
            }
        }

        public AutoMappingProfile()
        {
            this.conventions = new ConventionProfile();
            this.isSubClass = t => false;
            this.memberFinder = DefaultMemberFinder.Instance;
            this.overrides = new Dictionary<Type, ClassOverrides>();
        }

        public ClassOverrides GetOverridesFor(Type classType)
        {
            ClassOverrides classOverrides;
            if (!this.overrides.TryGetValue(classType, out classOverrides))
                classOverrides = this.overrides[classType] = new ClassOverrides();

            return classOverrides;
        }

        public IClassActivator GetClassActivator(Type classType)
        {
            return this.Conventions.ClassActivatorConvention.GetClassActivator(classType);
        }

        public string GetCollectionName(Type classType)
        {
            return this.GetClassOverrideValue<string>(classType,
                o => o.CollectionName,
                cn => !string.IsNullOrEmpty(cn),
                this.Conventions.CollectionNameConvention.GetCollectionName(classType));
        }

        public object GetDiscriminator(Type classType)
        {
            return this.Conventions.DiscriminatorConvention.GetDiscriminator(classType);
        }

        public string GetDiscriminatorKey(Type classType)
        {
            return this.Conventions.DiscriminatorKeyConvention.GetDiscriminatorKey(classType);
        }

        public IIdGenerator GetIdGenerator(Type classType, MemberInfo member)
        {
            return this.Conventions.IdGeneratorConvention.GetGenerator(ReflectionUtil.GetMemberValueType(member));
        }

        public object GetIdUnsavedValue(Type classType, MemberInfo member)
        {
            return this.Conventions.IdUnsavedValueConvention.GetUnsavedValue(ReflectionUtil.GetMemberValueType(member));
        }

        public string GetMemberKey(Type classType, MemberInfo member)
        {
            return GetMemberOverrideValue<string>(classType, member,
                o => o.Key,
                k => !string.IsNullOrEmpty(k),
                this.Conventions.MemberKeyConvention.GetMemberKey(member));
        }

        public IValueConverter GetValueConverter(Type classType, MemberInfo member)
        {
            return this.Conventions.ValueConverterConvention.GetValueConverter(ReflectionUtil.GetMemberValueType(member));
        }

        public bool IsReference(Type classType, MemberInfo member)
        {
            return GetMemberOverrideValue<bool>(classType, member,
                o => o.IsReference,
                b => true,
                false);
        }

        public bool ShouldMapMember(Type classType, MemberInfo member)
        {
            return GetMemberOverrideValue<bool>(classType, member,
                o => !o.Exclude,
                b => true,
                true);
        }

        private T GetClassOverrideValue<T>(Type classType, Func<ClassOverrides, T> overrides, Func<T, bool> accept, T defaultValue)
        {
            ClassOverrides classOverrides;
            if (!this.overrides.TryGetValue(classType, out classOverrides))
                return defaultValue;

            var value = overrides(classOverrides);
            if (!accept(value))
                return defaultValue;

            return value;
        }

        private T GetMemberOverrideValue<T>(Type classType, MemberInfo member, Func<MemberOverrides, T> overrides, Func<T, bool> accept, T defaultValue)
        {
            ClassOverrides classOverrides;
            if (!this.overrides.TryGetValue(classType, out classOverrides))
                return defaultValue;

            var value = overrides(classOverrides.GetOverridesFor(member));
            if (!accept(value))
                return defaultValue;

            return value;
        }
    }
}