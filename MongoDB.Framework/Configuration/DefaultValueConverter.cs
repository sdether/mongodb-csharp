using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration
{
    public class DefaultValueConverter : IValueConverter
    {
        public static readonly DefaultValueConverter Instance = new DefaultValueConverter();

        private DefaultValueConverter()
        { }

        public object ConvertFromDocumentValue(object documentValue)
        {
            if (documentValue == global::MongoDB.Driver.MongoDBNull.Value)
                documentValue = null;
            return documentValue;
        }

        public object ConvertToDocumentValue(object memberValue)
        {
            if (memberValue == null)
                memberValue = global::MongoDB.Driver.MongoDBNull.Value;
            return memberValue;
        }
    }
}