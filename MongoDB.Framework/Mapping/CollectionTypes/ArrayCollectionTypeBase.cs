using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.CollectionTypes
{
    public abstract class ArrayCollectionTypeBase : ICollectionType
    {
        /// <summary>
        /// Breaks the document value into elements.  This should not do any
        /// conversion of the elements themselves.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IEnumerable<object> BreakDocumentValueIntoElements(object value)
        {
            Array array = value as Array;
            if (array == null)
                return Enumerable.Empty<object>();

            return array.Cast<object>();
        }

        /// <summary>
        /// Creates the document value from elements.  This should not do any
        /// conversion of the elements themselves.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        public object CreateDocumentValueFromElements(IEnumerable<object> elements)
        {
            return elements.ToArray();
        }

        /// <summary>
        /// Creates a collection from the specified elements.
        /// </summary>
        /// <param name="elementType">Type of the element.</param>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        public abstract object CreateCollection(Type elementType, IEnumerable<object> elements);

        /// <summary>
        /// Gets the type of the collection.
        /// </summary>
        /// <param name="elementType">Type of the element.</param>
        /// <returns></returns>
        public abstract Type GetCollectionType(Type elementType);
    }
}