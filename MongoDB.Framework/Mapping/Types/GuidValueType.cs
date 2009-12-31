using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping.Types
{
    public class GuidValueType : NullSafeValueType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GuidValueType"/> class.
        /// </summary>
        public GuidValueType()
            : base(typeof(Guid))
        { }

        /// <summary>
        /// Converts from document value.
        /// </summary>
        /// <param name="documentValue">The document value.</param>
        /// <param name="mongoContext">The mongo context.</param>
        /// <returns></returns>
        public override object ConvertFromDocumentValue(object documentValue, IMongoContext mongoContext)
        {
            documentValue = base.ConvertFromDocumentValue(documentValue, mongoContext);
            var guid = documentValue as string;
            if (guid == null)
                return Guid.Empty;

            return new Guid(guid);
        }

        /// <summary>
        /// Converts to document value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="mongoContext">The mongo context.</param>
        /// <returns></returns>
        public override object ConvertToDocumentValue(object value, IMongoContext mongoContext)
        {
            value = base.ConvertToDocumentValue(value, mongoContext);
            if (value == MongoDBNull.Value)
                return value;

            return ((Guid)value).ToString();
        }
    }
}