using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper.Mapping.Conventions
{
    public class DelegateCollectionNameConvention : ICollectionNameConvention
    {
        private Func<Type, string> collectionName;

        public DelegateCollectionNameConvention(Func<Type, string> collectionName)
        {
            if (collectionName == null)
                throw new ArgumentNullException("collectionName");

            this.collectionName = collectionName;
        }

        public string GetCollectionName(Type type)
        {
            return this.collectionName(type);
        }
    }
}