using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration
{
    public class NoOpValueConverter : IValueConverter
    {
        public static readonly NoOpValueConverter Instance = new NoOpValueConverter();

        private NoOpValueConverter()
        { }

        public object ConvertFromDocumentValue(object documentValue)
        {
            return documentValue;
        }

        public object ConvertToDocumentValue(object memberValue)
        {
            return memberValue;
        }
    }
}