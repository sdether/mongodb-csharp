using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper.Mapping.Conventions
{
    public interface IValueConverterConvention
    {
        /// <summary>
        /// Gets the value converter.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        IValueConverter GetValueConverter(Type type);
    }
}