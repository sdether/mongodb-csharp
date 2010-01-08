using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping.ValueConverters
{
    public class NullSafeValueConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullSafeValueConverter"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public NullSafeValueConverter(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            this.Type = type;
        }

        /// <summary>
        /// Froms the document.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public virtual object FromDocument(object value)
        {
            if (value == null || value == MongoDBNull.Value)
                return this.Type.IsValueType ? Activator.CreateInstance(this.Type) : null;

            return value;
        }

        /// <summary>
        /// Toes the document.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public virtual object ToDocument(object value)
        {
            if (value == null)
                return MongoDBNull.Value;

            return value;
        }
    }
}