using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Configuration
{
    public class OidValueConverter : IValueConverter
    {
        public object ConvertFromDocumentValue(object documentValue)
        {
            Oid oid = documentValue as Oid;
            if (oid == null)
                throw new InvalidCastException(string.Format("Cannot convert {0} to an oid string.", documentValue));

            return BitConverter.ToString(oid.Value).Replace("-", "").ToLower();
        }

        public object ConvertToDocumentValue(object memberValue)
        {
            if (memberValue == null)
                return MongoDBNull.Value;

            string s = memberValue as string;
            if (s == null)
                throw new InvalidCastException("Oid member values must be of type string.");

            return new Oid(s);
        }
    }
}