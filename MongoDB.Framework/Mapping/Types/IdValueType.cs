using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping.Types
{
    public class IdValueType : NullSafeValueType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdValueType"/> class.
        /// </summary>
        public IdValueType()
            : base(typeof(string))
        { }

        /// <summary>
        /// Converts from document value.
        /// </summary>
        /// <param name="documentValue">The document value.</param>
        /// <returns></returns>
        public override object ConvertFromDocumentValue(object documentValue, MappingContext mappingContext)
        {
            documentValue = base.ConvertFromDocumentValue(documentValue, mappingContext);
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
        public override object ConvertToDocumentValue(object value)
        {
            value = base.ConvertToDocumentValue(value);
            if (value == MongoDBNull.Value)
                return value;

            return new Oid((string)value);
        }
    }
}