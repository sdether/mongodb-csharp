using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.ValueConverters
{
    public class StringKeyValueConverter<TValue> : IValueConverter
    {
        public Type Type
        {
            get { return typeof(KeyValuePair<string, TValue>); }
        }

        public object FromDocument(object value)
        {
            //we are expected a KeyValuePair<string, object>
            var kvp = (KeyValuePair<string, object>)value;
            return new KeyValuePair<string, TValue>(kvp.Key, (TValue)kvp.Value);
        }

        public object ToDocument(object value)
        {
            var kvp = (KeyValuePair<string, TValue>)value;
            return new KeyValuePair<string, object>(kvp.Key, kvp.Value);
        }
    }
}
