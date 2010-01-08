using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MongoDB.Framework.Mapping.CollectionTypes
{
    public abstract class ArrayCollectionTypeBase : ICollectionType
    {
        /// <summary>
        /// Breaks the collection into elements. This should not do any conversion
        /// of the elements themselves.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public IEnumerable<CollectionElement> BreakCollectionIntoElements(Type elementType, object collection)
        {
            foreach (var element in (IEnumerable)collection)
            {
                yield return new CollectionElement() { Element = element };
            }
        }

        /// <summary>
        /// Breaks the document value into elements.  This should not do any
        /// conversion of the elements themselves.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IEnumerable<CollectionElement> BreakDocumentValueIntoElements(object value)
        {
            Array array = value as Array;
            if (array == null)
                return Enumerable.Empty<CollectionElement>();

            return array.OfType<object>().Select(o => new CollectionElement() { Element = o });
        }

        /// <summary>
        /// Creates the document value from elements.  This should not do any
        /// conversion of the elements themselves.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        public object CreateDocumentValueFromElements(IEnumerable<CollectionElement> elements)
        {
            return elements.Select(e => e.Element).ToArray();
        }

        /// <summary>
        /// Creates a collection from the specified elements.
        /// </summary>
        /// <param name="elementType">Type of the element.</param>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        public abstract object CreateCollection(Type elementType, IEnumerable<CollectionElement> elements);

        /// <summary>
        /// Gets the type of the collection.
        /// </summary>
        /// <param name="elementType">Type of the element.</param>
        /// <returns></returns>
        public abstract Type GetCollectionType(Type elementType);
    }
}