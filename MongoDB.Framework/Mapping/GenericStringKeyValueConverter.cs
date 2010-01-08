using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    //public class GenericStringKeyValueConverter : IValueConverter
    //{
    //    private IValueConverter nestedConverter;

    //    public Type Type
    //    {
    //        get { return typeof(KeyValuePair<,>).MakeGenericType(typeof(string), this.nestedConverter.Type); }
    //    }

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="GenericStringKeyValueConverter"/> class.
    //    /// </summary>
    //    /// <param name="nestedConverter">The nested converter.</param>
    //    public GenericStringKeyValueConverter(IValueConverter nestedConverter)
    //    {
    //        if (nestedConverter == null)
    //            throw new ArgumentNullException("nestedConverter");

    //        this.nestedConverter = nestedConverter;
    //    }

    //    /// <summary>
    //    /// Froms the document.
    //    /// </summary>
    //    /// <param name="value">The value.</param>
    //    /// <returns></returns>
    //    public object FromDocument(object value)
    //    {
    //        return Activator.CreateInstance(this.Type, (
    //    }

    //    /// <summary>
    //    /// Toes the document.
    //    /// </summary>
    //    /// <param name="value">The value.</param>
    //    /// <returns></returns>
    //    public object ToDocument(object value)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}