using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public interface IValueConverter
    {
        Type Type { get; }

        /// <summary>
        /// Froms the document.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        object FromDocument(object value);

        /// <summary>
        /// Toes the document.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        object ToDocument(object value);
    }
}