using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using MongoDB.Driver;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Mapping.ValueConverters;
using MongoDB.Framework.Mapping.Conventions;
using MongoDB.Framework.Reflection;
using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Mapping.IdGenerators;

namespace MongoDB.Framework.Mapping.Auto
{
    public class AutoMapper : IAutoMapper
    {
        private Func<Type, bool> filter;
        private AutoMappingProfile profile;

        public AutoMapper()
            : this(null, null)
        { }

        public AutoMapper(Func<Type, bool> filter)
            : this(null, filter)
        { }

        public AutoMapper(AutoMappingProfile profile)
            : this(profile, null)
        { }

        public AutoMapper(AutoMappingProfile profile, Func<Type, bool> filter)
        {
            this.filter = filter ?? new Func<Type, bool>(t => true);
            this.profile = profile ?? new AutoMappingProfile();
        }

        public bool CanCreateClassMap(Type type)
        {
            return this.filter(type);
        }

        public ClassMapBase CreateClassMap(Type type, Func<Type, ClassMapBase> existingClassMapFinder)
        {
            if (!this.filter(type))
                throw new InvalidOperationException(string.Format("Cannot map type {0}. Ensure a call to CanCreateClassMap to avoid this exception.", type));

            if (this.profile.IsSubClass(type))
                return this.CreateSubClassMap(type, existingClassMapFinder);
            else
                return this.CreateClassMap(type);
        }

        private ClassMap CreateClassMap(Type type)
        {
            ClassMap classMap = new ClassMap(type);
            classMap.ClassActivator = this.profile.Conventions.ClassActivatorConvention.GetClassActivator(type);
            classMap.CollectionName = this.profile.Conventions.CollectionNameConvention.GetCollectionName(type);
            classMap.Discriminator = this.profile.Conventions.DiscriminatorConvention.GetDiscriminator(type);
            classMap.DiscriminatorKey = this.profile.Conventions.DiscriminatorKeyConvention.GetDiscriminatorKey(type);
            foreach(var member in this.profile.MemberFinder.FindMembers(type))
            {
                if (this.profile.Conventions.IdConvention.IsId(member))
                    classMap.IdMap = this.BuildIdMap(member);
                else if (this.profile.Conventions.ExtendedPropertiesConvention.IsExtendedProperties(member))
                    classMap.ExtendedPropertiesMap = this.BuildExtendedPropertiesMap(member);
                else
                    classMap.AddMemberMap(this.BuildMemberMap(member));
            }

            return classMap;
        }

        private SubClassMap CreateSubClassMap(Type type, Func<Type, ClassMapBase> existingClassMapFinder)
        {
            var superClassMap = existingClassMapFinder(type.BaseType);
            if (superClassMap == null)
                throw new InvalidOperationException(string.Format("Unable to find super class map for subclass {0}", type));
            if (superClassMap is SubClassMap)
                throw new NotSupportedException("2-level inheritance hierarchies are not currently supported.");

            var subClassMap = new SubClassMap(type);
            subClassMap.ClassActivator = this.profile.Conventions.ClassActivatorConvention.GetClassActivator(type);
            subClassMap.Discriminator = this.profile.Conventions.DiscriminatorConvention.GetDiscriminator(type);

            foreach (var member in this.profile.MemberFinder.FindMembers(type))
            {
                if(superClassMap.IdMap.MemberName == member.Name)
                    continue;
                if (superClassMap.ExtendedPropertiesMap.MemberName == member.Name)
                    continue;
                if (superClassMap.MemberMaps.Any(x => x.MemberName == member.Name))
                    continue;
                subClassMap.AddMemberMap(this.BuildMemberMap(member));
            }

            ((ClassMap)superClassMap).AddSubClassMap(subClassMap);
            return subClassMap;
        }

        private ExtendedPropertiesMap BuildExtendedPropertiesMap(MemberInfo member)
        {
            return new ExtendedPropertiesMap(
                member.Name,
                LateBoundReflection.GetGetter(member),
                LateBoundReflection.GetSetter(member));
        }

        private IdMap BuildIdMap(MemberInfo member)
        {
            var type = ReflectionUtil.GetMemberValueType(member);
            return new IdMap(
                member.Name,
                LateBoundReflection.GetGetter(member),
                LateBoundReflection.GetSetter(member),
                this.profile.Conventions.IdGeneratorConvention.GetGenerator(type),
                this.profile.Conventions.ValueConverterConvention.GetValueConverter(type),
                this.profile.Conventions.IdUnsavedValueConvention.GetUnsavedValue(type));
        }

        private MemberMap BuildMemberMap(MemberInfo member)
        {
            var type = ReflectionUtil.GetMemberValueType(member);
            return new ValueTypeMemberMap(
                this.profile.Conventions.MemberKeyConvention.GetMemberKey(member),
                member.Name,
                LateBoundReflection.GetGetter(member),
                LateBoundReflection.GetSetter(member),
                false,
                this.GetValueType(type));
        }

        private ValueTypeBase GetValueType(Type type)
        {
            if (IsNativeToMongo(type))
            {
                return new SimpleValueType(
                    type, 
                    this.profile.Conventions.ValueConverterConvention.GetValueConverter(type));
            }
            else if (this.profile.Conventions.CollectionValueTypeConvention.IsCollection(type))
            {
                return new CollectionValueType(
                    this.profile.Conventions.CollectionValueTypeConvention.GetCollectionType(type),
                    this.GetValueType(this.profile.Conventions.CollectionValueTypeConvention.GetElementType(type)));
            }
            else 
            {
                return new NestedClassValueType(
                    type, 
                    this.profile.Conventions.ValueConverterConvention.GetValueConverter(type));
            }

            throw new NotSupportedException(string.Format("The type {0} could not be mapped.", type));
        }

        private bool IsNativeToMongo(Type type)
        {
            var typeCode = Type.GetTypeCode(type);

            if (typeCode != TypeCode.Object)
                return true;

            if (type == typeof(Guid))
                return true;

            if (type == typeof(Oid))
                return true;

            return false;
        }
    }
}