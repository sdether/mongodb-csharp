using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Mapping;
using MongoDB.Framework.Mapping.CollectionTypes;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public class DefaultCollectionConvention : ConventionBase<Type>, ICollectionConvention
    {
        public static readonly DefaultCollectionConvention AlwaysMatching = new DefaultCollectionConvention();

        private DefaultCollectionConvention()
            : base(t => true)
        { }

        public ICollectionType GetCollectionType(Type type)
        {
            if (type.IsGenericType)
            {
                var genType = type.GetGenericTypeDefinition();
                if (genType == typeof(IList<>) || genType == typeof(List<>) || genType == typeof(ICollection<>))
                    return new GenericListCollectionType();
                if (genType == typeof(HashSet<>))
                    return new HashSetCollectionType();
                if (genType == typeof(IDictionary<,>) || genType == typeof(Dictionary<,>) && type.GetGenericArguments()[0] == typeof(string))
                    return new GenericStringDictionaryCollectionType();
            }

            throw new NotSupportedException(string.Format("Could not create collection type from {0}.", type));
        }

        public Type GetElementType(Type type)
        {
            if (type.IsGenericType)
            {
                var genType = type.GetGenericTypeDefinition();
                if (genType == typeof(IList<>) || genType == typeof(List<>) || genType == typeof(ICollection<>) || genType == typeof(HashSet<>))
                    return type.GetGenericArguments()[0];
                if (genType == typeof(IDictionary<,>) || genType == typeof(Dictionary<,>) && type.GetGenericArguments()[0] == typeof(string))
                    return type.GetGenericArguments()[1];
            }

            throw new NotSupportedException(string.Format("Could not discover element type from {0}.", type));
        }

        public bool IsCollection(Type type)
        {
            if (type.IsGenericType)
            {
                var genType = type.GetGenericTypeDefinition();
                return genType == typeof(IList<>)
                    || genType == typeof(List<>)
                    || genType == typeof(ICollection<>)
                    || genType == typeof(HashSet<>)
                    || ((genType == typeof(IDictionary<,>) || genType == typeof(Dictionary<,>)) && type.GetGenericArguments()[0] == typeof(string));
            }

            return false;
        }
    }
}