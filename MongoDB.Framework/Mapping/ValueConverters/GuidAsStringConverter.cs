using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping.ValueConverters
{
    public class GuidAsStringValueConverter : IValueConverter
    {
        private string format;
 
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type
        {
            get { return typeof(Guid); }
        }
 
        /// <summary>
        /// Initializes a new instance of the <see cref="GuidAsStringValueConverter"/> class.
        /// </summary>
        public GuidAsStringValueConverter()
            : this("N")
        { }
 
        /// <summary>
        /// Initializes a new instance of the <see cref="GuidAsStringValueConverter"/> class.
        /// </summary>
        /// <param name="format">The format.</param>
        public GuidAsStringValueConverter(string format)
        {
            this.format = format ?? "N";
        }

        /// <summary>
        /// Froms the document.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public object FromDocument(object value)
        {
            var str = value as string;
            if (str != null)
                return new Guid(str);
 
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
 
            return ((Guid)value).ToString(this.format);
        }
    }
}