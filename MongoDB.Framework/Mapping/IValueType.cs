﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping
{
    public interface IValueType
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        Type Type { get; }

        /// <summary>
        /// Converts from document value.
        /// </summary>
        /// <param name="documentValue">The document value.</param>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns></returns>
        object ConvertFromDocumentValue(object documentValue, IMappingContext mappingContext);

        /// <summary>
        /// Converts to document value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns></returns>
        object ConvertToDocumentValue(object value, IMappingContext mappingContext);
    }
}