using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using MongoDB.Driver;

namespace MongoDB.Framework.Mapping
{
    public static class MongoTypeConverter
    {
        public static object ConvertFromDocumentValue(object documentValue)
        {
            if (documentValue == null || documentValue == MongoDBNull.Value)
                return null;
            else if (documentValue is MongoRegex)
            {
                var mongoRegex = (MongoRegex)documentValue;
                return new Regex(mongoRegex.Expression);
            }
            else if (documentValue is Oid)
            {
                var oid = (Oid)documentValue;
                return BitConverter.ToString(oid.Value).Replace("-", "").ToLower();
            }

            return documentValue;
        }

        public static object ConvertToDocumentValue(Type entityValueType, object entityValue)
        {
            if(entityValue == null)
                return MongoDBNull.Value;
            else if (entityValueType == typeof(Regex))
            {
                var regex = (MongoRegex)entityValue;
                return new MongoRegex(regex.Expression);
            }

            return entityValue;
        }

        public static object ConvertToOid(string id)
        {
            if (id == null)
                return MongoDBNull.Value;

            return new Oid(id);
        }
    }
}