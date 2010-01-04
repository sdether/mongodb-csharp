using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using System.Collections;

namespace MongoDB.Framework.Configuration.Mapping.Types
{
    public class DictionaryCollectionType : ICollectionType
    {
        public Type GetCollectionType(IValueType elementValueType)
        {
            return typeof(Dictionary<,>).MakeGenericType(new[] { typeof(string), elementValueType.Type });
        }

        /// <summary>
        /// Converts from document value.
        /// </summary>
        /// <param name="elementValueType">Type of the element value.</param>
        /// <param name="documentValue">The document value.</param>
        /// <param name="mongoContext">The mongo context.</param>
        /// <returns></returns>
        public object ConvertFromDocumentValue(IValueType elementValueType, object documentValue, IMongoContextImplementor mongoContext)
        {
            Document document = documentValue as Document;
            if (document == null)
                return null;
            
            var dictionary = Activator.CreateInstance(this.GetCollectionType(elementValueType));
            var addMethod = dictionary.GetType().GetMethod("Add", new[] { typeof(string), elementValueType.Type });
            foreach (string key in document.Keys)
                addMethod.Invoke(dictionary, new[] { key, elementValueType.ConvertFromDocumentValue(document[key], mongoContext) });

            return dictionary;
        }

        /// <summary>
        /// Converts to document value.
        /// </summary>
        /// <param name="elementValueType">Type of the element value.</param>
        /// <param name="value">The value.</param>
        /// <param name="mongoContext">The mongo context.</param>
        /// <returns></returns>
        public object ConvertToDocumentValue(IValueType elementValueType, object value, IMongoContextImplementor mongoContext)
        {
            Document document = new Document();
            var enumerable = value as IEnumerable;
            if (enumerable == null)
                return null;

            var keyValuePairType = typeof(KeyValuePair<,>).MakeGenericType(new [] { typeof(string), elementValueType.Type });
            var keyProperty = keyValuePairType.GetProperty("Key");
            var valueProperty = keyValuePairType.GetProperty("Value");
            foreach(object pair in enumerable)
            {
                document.Add(
                    (string)keyProperty.GetValue(pair, null),
                    elementValueType.ConvertToDocumentValue(valueProperty.GetValue(pair, null), mongoContext));
            }

            return document;
        }
    }
}
