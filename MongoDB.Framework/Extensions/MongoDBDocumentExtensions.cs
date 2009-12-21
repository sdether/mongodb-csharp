using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;

namespace MongoDB.Framework
{
    public static class MongoDBDocumentExtensions
    {
        public static IDictionary<string, object> ToDictionary(this Document document)
        {
            var dictionary = new Dictionary<string, object>();
            foreach (string key in document.Keys)
            {
                object value = document[key];
                if (value is Document)
                {
                    var subDictionary = new Dictionary<string, object>();
                    value = ((Document)value).ToDictionary();
                }
                dictionary.Add(key, value);
            }
            return dictionary;
        }
    }
}