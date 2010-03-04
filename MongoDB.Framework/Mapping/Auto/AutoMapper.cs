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
        private AutoMapperSetup setup;

        public AutoMapper()
            : this(null)
        { }

        public AutoMapper(AutoMapperSetup setup)
        {
            this.setup = setup ?? new AutoMapperSetup();
        }

        public ClassMapBase CreateClassMap(Type type, Func<Type, ClassMapBase> existingClassMapFinder)
        {
            if (this.setup.IsSubClass(type))
                return this.CreateSubClassMap(type, existingClassMapFinder);
            else
                return this.CreateClassMap(type);
        }

        private ClassMap CreateClassMap(Type type)
        {
            ClassMap classMap = new ClassMap(type);
            classMap.ClassActivator = this.setup.Conventions.ClassActivatorConvention.GetClassActivator(type);
            classMap.CollectionName = this.setup.Conventions.CollectionNameConvention.GetCollectionName(type);

            foreach(var member in this.setup.MemberFinder.FindMembers(type))
            {
                if (this.setup.Conventions.IdConvention.IsId(member))
                    classMap.IdMap = this.BuildIdMap(member);
                else if (this.setup.Conventions.ExtendedPropertiesConvention.IsExtendedProperties(member))
                    classMap.ExtendedPropertiesMap = this.BuildExtendedPropertiesMap(member);
                else
                    classMap.AddMemberMap(this.BuildMemberMap(member));
            }

            return classMap;
        }

        private ClassMap CreateSubClassMap(Type type, Func<Type, ClassMapBase> existingClassMapFinder)
        {
            //var sub = new SubClassMap(type);
            //sub.SuperClassMap = existingClassMapFinder.
            throw new NotSupportedException();
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
                this.setup.Conventions.IdGeneratorConvention.GetGenerator(type),
                this.setup.Conventions.ValueConverterConvention.GetValueConverter(type),
                this.setup.Conventions.IdUnsavedValueConvention.GetUnsavedValue(type));
        }

        private MemberMap BuildMemberMap(MemberInfo member)
        {
            var type = ReflectionUtil.GetMemberValueType(member);
            return new ValueTypeMemberMap(
                this.setup.Conventions.MemberKeyConvention.GetMemberKey(member),
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
                    this.setup.Conventions.ValueConverterConvention.GetValueConverter(type));
            }
            else if (this.setup.Conventions.CollectionValueTypeConvention.IsCollection(type))
            {
                return new CollectionValueType(
                    this.setup.Conventions.CollectionValueTypeConvention.GetCollectionType(type),
                    this.GetValueType(this.setup.Conventions.CollectionValueTypeConvention.GetElementType(type)));
            }
            else 
            {
                return new NestedClassValueType(
                    type, 
                    this.setup.Conventions.ValueConverterConvention.GetValueConverter(type));
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