using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using System.Collections;

namespace MongoDB.Framework.Mapping.Types
{
    public class DictionaryCollectionType : ICollectionType
    {
        public Type GetCollectionType(IValueType elementValueType)
        {
            return typeof(Dictionary<,>).MakeGenericType(new[] { typeof(string), elementValueType.Type });
        }

        public object ConvertFromDocumentValue(IValueType elementValueType, object documentValue, MappingContext mappingContext)
        {
            Document document = documentValue as Document;
            if (document == null)
                return null;
            
            var dictionary = Activator.CreateInstance(this.GetCollectionType(elementValueType));
            var addMethod = dictionary.GetType().GetMethod("Add", new[] { typeof(string), elementValueType.Type });
            foreach (string key in document.Keys)
                addMethod.Invoke(dictionary, new[] { key, elementValueType.ConvertFromDocumentValue(document[key], mappingContext) });

            return dictionary;
        }

        public object ConvertToDocumentValue(IValueType elementValueType, object value)
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
                    elementValueType.ConvertToDocumentValue(valueProperty.GetValue(pair, null)));
            }

            return document;
        }
    }
}
