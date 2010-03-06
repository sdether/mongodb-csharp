using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using MongoDB.Driver;
using MongoDB.Mapper.Mapping;
using MongoDB.Mapper.Mapping.ValueConverters;
using MongoDB.Mapper.Mapping.Conventions;
using MongoDB.Mapper.Reflection;
using MongoDB.Mapper.Configuration.Mapping;
using MongoDB.Mapper.Mapping.IdGenerators;

namespace MongoDB.Mapper.Mapping.Auto
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

        private ClassMap CreateClassMap(Type classType)
        {
            ClassMap classMap = new ClassMap(classType);
            classMap.ClassActivator = this.profile.GetClassActivator(classType);
            classMap.CollectionName = this.profile.GetCollectionName(classType);
            if(!classType.IsInterface && !classType.IsAbstract)
                classMap.Discriminator = this.profile.GetDiscriminator(classType);
            classMap.DiscriminatorKey = this.profile.GetDiscriminatorKey(classType);
            foreach(var member in this.profile.MemberFinder.FindMembers(classType))
            {
                if (!this.profile.ShouldMapMember(classType, member))
                    continue;
                if (this.profile.Conventions.IdConvention.IsId(member))
                    classMap.IdMap = this.BuildIdMap(classType, member);
                else if (this.profile.Conventions.ExtendedPropertiesConvention.IsExtendedProperties(member))
                    classMap.ExtendedPropertiesMap = this.BuildExtendedPropertiesMap(classType, member);
                else
                    classMap.AddMemberMap(this.BuildMemberMap(classType, member));
            }

            return classMap;
        }

        private SubClassMap CreateSubClassMap(Type classType, Func<Type, ClassMapBase> existingClassMapFinder)
        {
            var superClassMap = existingClassMapFinder(classType.BaseType);
            if (superClassMap == null)
                throw new InvalidOperationException(string.Format("Unable to find super class map for subclass {0}", classType));
            if (superClassMap is SubClassMap)
                throw new NotSupportedException("2-level inheritance hierarchies are not currently supported.");

            var subClassMap = new SubClassMap(classType);
            subClassMap.ClassActivator = this.profile.GetClassActivator(classType);
            subClassMap.Discriminator = this.profile.GetDiscriminator(classType);

            foreach (var member in this.profile.MemberFinder.FindMembers(classType))
            {
                if (!this.profile.ShouldMapMember(classType, member))
                    continue;
                if(superClassMap.HasId && superClassMap.IdMap.MemberName == member.Name)
                    continue;
                if (superClassMap.HasExtendedProperties && superClassMap.ExtendedPropertiesMap.MemberName == member.Name)
                    continue;
                if (superClassMap.MemberMaps.Any(x => x.MemberName == member.Name))
                    continue;
                subClassMap.AddMemberMap(this.BuildMemberMap(classType, member));
            }

            ((ClassMap)superClassMap).AddSubClassMap(subClassMap);
            return subClassMap;
        }

        private ExtendedPropertiesMap BuildExtendedPropertiesMap(Type classType, MemberInfo member)
        {
            return new ExtendedPropertiesMap(
                member.Name,
                LateBoundReflection.GetGetter(member),
                LateBoundReflection.GetSetter(member));
        }

        private IdMap BuildIdMap(Type classType, MemberInfo member)
        {
            return new IdMap(
                member.Name,
                LateBoundReflection.GetGetter(member),
                LateBoundReflection.GetSetter(member),
                this.profile.GetIdGenerator(classType, member),
                this.profile.GetValueConverter(classType, member),
                this.profile.GetIdUnsavedValue(classType, member));
        }

        private MemberMap BuildMemberMap(Type classType, MemberInfo member)
        {
            return new ValueTypeMemberMap(
                this.profile.GetMemberKey(classType, member),
                member.Name,
                LateBoundReflection.GetGetter(member),
                LateBoundReflection.GetSetter(member),
                false,
                this.GetValueType(classType, member));
        }

        private ValueTypeBase GetValueType(Type classType, MemberInfo member)
        {
            return this.GetValueType(classType, member, ReflectionUtil.GetMemberValueType(member));
        }

        private ValueTypeBase GetValueType(Type classType, MemberInfo member, Type memberType)
        {
            if (this.profile.IsReference(classType, member))
            {
                return new ManyToOneValueType(
                    memberType,
                    true);
            }
            else if (IsNativeToMongo(memberType))
            {
                return new SimpleValueType(
                    memberType,
                    this.profile.GetValueConverter(classType, member));
            }
            else if (this.profile.Conventions.CollectionValueTypeConvention.IsCollection(memberType))
            {
                var collectionType =  this.profile.Conventions.CollectionValueTypeConvention.GetCollectionType(memberType);
                var elementType = this.profile.Conventions.CollectionValueTypeConvention.GetElementType(memberType);
                return new CollectionValueType(
                   collectionType,
                   this.GetValueType(classType, member, elementType));
            }
            else 
            {
                return new NestedClassValueType(
                    memberType,
                    this.profile.GetValueConverter(classType, member));
            }

            throw new NotSupportedException(string.Format("The type {0} could not be mapped.", memberType));
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