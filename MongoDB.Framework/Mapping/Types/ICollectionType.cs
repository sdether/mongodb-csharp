using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping.Types;

namespace MongoDB.Framework.Mapping
{
    public interface ICollectionType
    {
        /// <summary>
        /// Gets the type of the collection.
        /// </summary>
        /// <param name="elementValueType">Type of the element value.</param>
        /// <returns></returns>
        Type GetCollectionType(IValueType elementValueType);

        /// <summary>
        /// Creates the collection.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        object CreateCollection(IValueType elementValueType, IList<object> elements);
    }
}