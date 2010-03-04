using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping;
using System.Reflection;
using MongoDB.Driver;
using MongoDB.Framework.Reflection;
using MongoDB.Framework.Mapping.ValueConverters;

namespace MongoDB.Framework.Mapping.Auto
{
    public class AutoMapper
    {
        private Func<Type, ClassMapBase> existingClassMapFinder;

        public AutoMapper(Func<Type, ClassMapBase> existingClassMapFinder)
        {
            this.existingClassMapFinder = existingClassMapFinder;
        }

        public ClassMapBase CreateClassMapFor(Type type)
        {
            var classMap = this.existingClassMapFinder(type);
            if (classMap != null)
                return classMap;

            return null;
        }

        private IEnumerable<MemberMap> GetMemberMapsFor(Type type)
        {
            foreach (var member in type.GetMembers(BindingFlags.Instance | BindingFlags.Public))
            {
            }
            return null;
        }

        private bool IsCollection(Type type)
        {
            return false;
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