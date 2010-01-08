using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public interface ICollectionType
    {
        /// <summary>
        /// Breaks the collection into elements. This should not do any conversion
        /// of the elements themselves.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        IEnumerable<CollectionElement> BreakCollectionIntoElements(Type elementType, object collection);

        /// <summary>
        /// Breaks the document value into elements.  This should not do any 
        /// conversion of the elements themselves.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IEnumerable<CollectionElement> BreakDocumentValueIntoElements(object value);

        /// <summary>
        /// Creates the document value from elements.  This should not do any
        /// conversion of the elements themselves.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        object CreateDocumentValueFromElements(IEnumerable<CollectionElement> elements);

        /// <summary>
        /// Creates a collection from the specified elements.
        /// </summary>
        /// <param name="elementType">Type of the element.</param>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        object CreateCollection(Type elementType, IEnumerable<CollectionElement> elements);

        /// <summary>
        /// Gets the type of the collection.
        /// </summary>
        /// <param name="elementType">Type of the element.</param>
        /// <returns></returns>
        Type GetCollectionType(Type elementType);
    }
}