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
        /// <returns></returns>
        public override object ConvertFromDocumentValue(object documentValue, MappingContext mappingContext)
        {
            documentValue = base.ConvertFromDocumentValue(documentValue, mappingContext);
            var guid = documentValue as string;
            if (guid == null)
                return null;

            return new Guid(guid);
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

            return ((Guid)value).ToString();
        }
    }
}