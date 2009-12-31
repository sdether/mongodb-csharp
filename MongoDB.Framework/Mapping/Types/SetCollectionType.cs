using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace MongoDB.Framework.Mapping.Types
{
    public class SetCollectionType : ICollectionType
    {
        /// <summary>
        /// Gets the type of the collection.
        /// </summary>
        /// <param name="elementValueType">Type of the element value.</param>
        /// <returns></returns>
        public Type GetCollectionType(IValueType elementValueType)
        {
            return typeof(HashSet<>).MakeGenericType(elementValueType.Type);
        }

        /// <summary>
        /// Converts from document value.
        /// </summary>
        /// <param name="elementValueType">Type of the element value.</param>
        /// <param name="documentValue">The document value.</param>
        /// <param name="mongoContext">The mongo context.</param>
        /// <returns></returns>
        public object ConvertFromDocumentValue(IValueType elementValueType, object documentValue, IMongoContext mongoContext)
        {
            Array array = documentValue as Array;
            if (array == null)
                return null;

            var list = Activator.CreateInstance(this.GetCollectionType(elementValueType));
            var addMethod = list.GetType().GetMethod("Add", new[] { elementValueType.Type });
            foreach (var element in array)
                addMethod.Invoke(list, new[] { elementValueType.ConvertFromDocumentValue(element, mongoContext) });

            return list;
        }

        /// <summary>
        /// Converts to document value.
        /// </summary>
        /// <param name="elementValueType">Type of the element value.</param>
        /// <param name="value">The value.</param>
        /// <param name="mappingContext"></param>
        /// <returns></returns>
        public object ConvertToDocumentValue(IValueType elementValueType, object value, IMongoContext mongoContext)
        {
            var enumerableValue = value as IEnumerable;
            if (enumerableValue == null)
                return null;

            return enumerableValue.OfType<object>()
                .Select(e => elementValueType.ConvertToDocumentValue(e, mongoContext))
                .ToArray();
        }
    }
}