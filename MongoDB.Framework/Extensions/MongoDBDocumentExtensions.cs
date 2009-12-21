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

        /// <summary>
        /// Converts the dictionary to document.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="document">The document.</param>
        public static Document ToDocument(this IDictionary<string, object> dictionary)
        {
            var document = new Document();
            foreach (var kvp in dictionary)
            {
                if (kvp.Value is IDictionary<string, object>)
                {
                    var subDocument = ((IDictionary<string, object>)kvp.Value).ToDocument();
                    document.Add(kvp.Key, subDocument);
                }
                else
                    document.Add(kvp.Key, kvp.Value);
            }
            return document;
        }
    }
}