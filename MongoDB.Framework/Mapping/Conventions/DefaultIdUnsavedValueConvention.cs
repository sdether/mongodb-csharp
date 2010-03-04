using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Conventions
{
    public class DefaultIdUnsavedValueConvention : IIdUnsavedValueConvention
    {
        public static readonly DefaultIdUnsavedValueConvention Instance = new DefaultIdUnsavedValueConvention();

        private DefaultIdUnsavedValueConvention()
        { }

        public object GetUnsavedValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}