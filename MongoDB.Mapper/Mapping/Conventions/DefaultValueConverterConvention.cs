using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Mapper.Mapping.ValueConverters;

namespace MongoDB.Mapper.Mapping.Conventions
{
    public class DefaultValueConverterConvention : IValueConverterConvention
    {
        public static readonly DefaultValueConverterConvention Instance = new DefaultValueConverterConvention();

        private DefaultValueConverterConvention()
        { }

        /// <summary>
        /// Gets the value converter.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public IValueConverter GetValueConverter(Type type)
        {
            return new NullSafeValueConverter(type);
        }
    }
}