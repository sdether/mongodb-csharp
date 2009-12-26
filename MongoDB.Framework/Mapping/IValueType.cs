using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping
{
    public interface IValueType
    {
        /// <summary>
        /// Converts from document value.
        /// </summary>
        /// <param name="documentValue">The document value.</param>
        /// <param name="translationContext">The translation context.</param>
        /// <returns></returns>
        object ConvertFromDocumentValue(object documentValue, TranslationContext translationContext);

        /// <summary>
        /// Converts to document value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="owner">The owner.</param>
        /// <returns></returns>
        object ConvertToDocumentValue(object value, TranslationContext translationContext);
    }
}