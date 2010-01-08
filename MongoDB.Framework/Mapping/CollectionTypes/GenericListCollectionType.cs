using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.CollectionTypes
{
    public class GenericListCollectionType : ArrayCollectionTypeBase
    {
        public override object CreateCollection(Type elementType, IEnumerable<CollectionElement> elements)
        {
            var instance = Activator.CreateInstance(this.GetCollectionType(elementType));

            var method = instance.GetType().GetMethod("Add", new Type[] { elementType });

            foreach (var element in elements)
                method.Invoke(instance, new[] { element.Element });

            return instance;
        }

        public override Type GetCollectionType(Type elementType)
        {
            return typeof(List<>).MakeGenericType(elementType);
        }
    }
}