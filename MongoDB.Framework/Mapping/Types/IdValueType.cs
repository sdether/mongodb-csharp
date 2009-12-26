using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping.Types
{
    public class IdValueType : NullSafeValueType
    {
        public IdValueType()
            : base(typeof(string))
        { }

        /// <summary>
        /// Converts from document value.
        /// </summary>
        /// <param name="documentValue">The document value.</param>
        /// <returns></returns>
        public override object ConvertFromDocumentValue(object documentValue, TranslationContext translationContext)
        {
            documentValue = base.ConvertFromDocumentValue(documentValue, translationContext);
            var oid = documentValue as Oid;
            if (oid == null)
                return null;

            return BitConverter.ToString(oid.Value).Replace("-", "").ToLower();
        }

        /// <summary>
        /// Converts to document value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="owner">The owner.</param>
        /// <returns></returns>
        public override object ConvertToDocumentValue(object value, TranslationContext translationContext)
        {
            value = base.ConvertToDocumentValue(value, translationContext);
            if (value == MongoDBNull.Value)
                return value;

            return new Oid((string)value);
        }
    }
}