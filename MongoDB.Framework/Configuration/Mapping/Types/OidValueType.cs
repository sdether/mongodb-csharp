using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Configuration.Mapping.Types
{
    public class OidValueType : NullSafeValueType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdValueType"/> class.
        /// </summary>
        public OidValueType()
            : base(typeof(string))
        { }

        /// <summary>
        /// Converts from document value.
        /// </summary>
        /// <param name="documentValue">The document value.</param>
        /// <param name="mongoContext">The mongo context.</param>
        /// <returns></returns>
        public override object ConvertFromDocumentValue(object documentValue, IMongoContextImplementor mongoContext)
        {
            documentValue = base.ConvertFromDocumentValue(documentValue, mongoContext);
            var oid = documentValue as Oid;
            if (oid == null)
                return null;

            return BitConverter.ToString(oid.Value).Replace("-", "").ToLower();
        }

        /// <summary>
        /// Converts to document value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="mongoContext">The mongo context.</param>
        /// <returns></returns>
        public override object ConvertToDocumentValue(object value, IMongoContextImplementor mongoContext)
        {
            value = base.ConvertToDocumentValue(value, mongoContext);
            if (value == MongoDBNull.Value)
                return value;

            return new Oid((string)value);
        }
    }
}