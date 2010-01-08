using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using System.Collections;

namespace MongoDB.Framework.Mapping.CollectionTypes
{
    public class GenericDictionaryCollectionType : ICollectionType
    {
        public IEnumerable<object> BreakDocumentValueIntoElements(object value)
        {
            var document = value as Document;
            if (document == null)
                return Enumerable.Empty<object>();

            var elements = new List<object>();
            foreach (string key in document.Keys)
                elements.Add(new KeyValuePair<string, object>(key, document[key]));

            return elements;
        }

        public object CreateDocumentValueFromElements(IEnumerable<object> elements)
        {
            Document document = new Document();
            foreach (KeyValuePair<string, object> element in elements)
                document.Add(element.Key, element.Value);

            return document;
        }

        public object CreateCollection(Type elementType, IEnumerable<object> elements)
        {
            var instance = Activator.CreateInstance(this.GetCollectionType(elementType));

            elementType = typeof(KeyValuePair<,>).MakeGenericType(typeof(string), elementType);
            var collectionType = typeof(ICollection<>).MakeGenericType(elementType);
            var method = collectionType.GetMethod("Add", new Type[] { elementType });

            foreach (var element in elements)
                method.Invoke(instance, new[] { element });

            return instance;
        }

        public Type GetCollectionType(Type elementType)
        {
            return typeof(Dictionary<,>).MakeGenericType(typeof(string), elementType);
        }
    }
}