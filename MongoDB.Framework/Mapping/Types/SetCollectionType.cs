using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Mapping.Types
{
    public class SetCollectionType : ICollectionType
    {
        /// <summary>
        /// Gets the type of the collection.
        /// </summary>
        /// <param name="elementValueType">Type of the element value.</param>
        /// <returns></returns>
        public Type GetCollectionType(IValueType elementValueType)
        {
            return typeof(HashSet<>).MakeGenericType(elementValueType.Type);
        }

        /// <summary>
        /// Creates the collection.
        /// </summary>
        /// <param name="elementValueType"></param>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        public object CreateCollection(IValueType elementValueType, IList<object> elements)
        {
            var set = Activator.CreateInstance(this.GetCollectionType(elementValueType));
            if (elements.Count == 0)
                return set;

            var addMethod = set.GetType().GetMethod("Add", new[] { elementValueType.Type });
            foreach (var element in elements)
                addMethod.Invoke(set, new[] { element });
            return set;
        }
    }
}