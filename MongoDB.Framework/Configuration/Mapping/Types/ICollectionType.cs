using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Configuration.Mapping.Types;

namespace MongoDB.Framework.Configuration.Mapping
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
        /// Converts from document value.
        /// </summary>
        /// <param name="elementValueType">Type of the element value.</param>
        /// <param name="documentValue">The document value.</param>
        /// <param name="mongoContext">The mongo context.</param>
        /// <returns></returns>
        object ConvertFromDocumentValue(IValueType elementValueType, object documentValue, IMongoContext mongoContext);

        /// <summary>
        /// Converts to document value.
        /// </summary>
        /// <param name="elementValueType">Type of the element value.</param>
        /// <param name="value">The value.</param>
        /// <param name="mongoContext">The mongo context.</param>
        /// <returns></returns>
        object ConvertToDocumentValue(IValueType elementValueType, object value, IMongoContext mongoContext);
    }
}