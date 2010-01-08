using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using System.Collections;

namespace MongoDB.Framework.Mapping.CollectionTypes
{
    public class GenericStringDictionaryCollectionType : ICollectionType
    {
        public IEnumerable<CollectionElement> BreakCollectionIntoElements(Type elementType, object collection)
        {
            elementType = typeof(KeyValuePair<,>).MakeGenericType(typeof(string), elementType);
            var keyProperty = elementType.GetProperty("Key");
            var valueProperty = elementType.GetProperty("Value");
            foreach (var element in (IEnumerable)collection)
            {
                yield return new CollectionElement()
                {
                    CustomData = keyProperty.GetValue(element, null),
                    Element = valueProperty.GetValue(element, null)
                };
            }
        }

        public IEnumerable<CollectionElement> BreakDocumentValueIntoElements(object value)
        {
            var document = value as Document;
            if (document == null)
                return Enumerable.Empty<CollectionElement>();

            var elements = new List<CollectionElement>();
            foreach (string key in document.Keys)
                elements.Add(new CollectionElement() { CustomData = key, Element = document[key] });

            return elements;
        }

        public object CreateDocumentValueFromElements(IEnumerable<CollectionElement> elements)
        {
            Document document = new Document();
            foreach (var element in elements)
                document.Add((string)element.CustomData, element.Element);

            return document;
        }

        public object CreateCollection(Type elementType, IEnumerable<CollectionElement> elements)
        {
            var collectionType = this.GetCollectionType(elementType);
            var instance = Activator.CreateInstance(collectionType);

            var method = collectionType.GetMethod("Add", new Type[] { typeof(string), elementType });

            foreach (var element in elements)
                method.Invoke(instance, new [] { (string)element.CustomData, element.Element });

            return instance;
        }

        public Type GetCollectionType(Type elementType)
        {
            return typeof(Dictionary<,>).MakeGenericType(typeof(string), elementType);
        }
    }
}