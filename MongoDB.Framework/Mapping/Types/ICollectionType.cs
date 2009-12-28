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
        /// <value>The type of the collection.</value>
        Type CollectionType { get; }

        /// <summary>
        /// Gets the type of the element value.
        /// </summary>
        /// <value>The type of the element value.</value>
        IValueType ElementValueType { get; }

        /// <summary>
        /// Creates the collection.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        object CreateCollection(IEnumerable<object> elements);
    }
}