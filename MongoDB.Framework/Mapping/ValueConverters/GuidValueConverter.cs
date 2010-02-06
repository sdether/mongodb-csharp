using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping.ValueConverters
{
    public class GuidValueConverter : IValueConverter
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type
        {
            get { return typeof(Guid); }
        }

        /// <summary>
        /// Froms the document.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public object FromDocument(object value)
        {
            var bin = value as Binary;
            if (bin != null)
                return new Guid(bin.Bytes);

            return Guid.Empty;
        }

        /// <summary>
        /// Toes the document.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public object ToDocument(object value)
        {
            if (value == null)
                return MongoDBNull.Value;

            return new Binary(((Guid)value).ToByteArray())
            {
                Subtype = Binary.TypeCode.Uuid
            };
        }
    }
}