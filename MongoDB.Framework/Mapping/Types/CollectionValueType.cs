using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MongoDB.Framework.Mapping.Types
{
    public abstract class CollectionValueType : IValueType
    {
        public abstract Type CollectionType { get; }

        public Type[] GenericArguments { get; set; }

        public IValueType ElementValueType { get; set; }

        public CollectionValueType()
        { }

        public object ConvertFromDocumentValue(object documentValue, MappingContext mappingContext)
        {
            Array array = documentValue as Array;
            if (array == null)
                return null;

            var collection = this.CreateCollection();
            var addMethod = this.CollectionType.GetMethod("Add");
            foreach (var element in array)
                addMethod.Invoke(collection, new [] { this.ElementValueType.ConvertFromDocumentValue(element, mappingContext) });

            return collection;
        }

        public object ConvertToDocumentValue(object value)
        {
            var enumerableValue = value as IEnumerable;
            return enumerableValue.OfType<object>()
                .Select(e => this.ElementValueType.ConvertToDocumentValue(e))
                .ToArray();
        }

        private object CreateCollection()
        {
            Type type = this.CollectionType;
            if (type.IsGenericTypeDefinition)
                type = this.CollectionType.MakeGenericType(this.GenericArguments);

            return Activator.CreateInstance(type);
        }
    }
}