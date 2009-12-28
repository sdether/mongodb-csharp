using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MongoDB.Framework.Mapping.Types
{
    public class CollectionValueType : IValueType
    {
        private ICollectionType collectionType;

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type
        {
            get { return collectionType.CollectionType; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionValueType"/> class.
        /// </summary>
        /// <param name="collectionType">Type of the collection.</param>
        public CollectionValueType(ICollectionType collectionType)
        {
            if (collectionType == null)
                throw new ArgumentNullException("collectionType");

            this.collectionType = collectionType;
        }

        /// <summary>
        /// Converts from document value.
        /// </summary>
        /// <param name="documentValue">The document value.</param>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns></returns>
        public object ConvertFromDocumentValue(object documentValue, MappingContext mappingContext)
        {
            Array array = documentValue as Array;
            if (array == null)
                return null;

            var elements = new List<object>();
            foreach (var element in array)
                elements.Add(this.collectionType.ElementValueType.ConvertFromDocumentValue(element, mappingContext));

            return this.collectionType.CreateCollection(elements);
        }

        /// <summary>
        /// Converts to document value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public object ConvertToDocumentValue(object value)
        {
            var enumerableValue = value as IEnumerable;
            return enumerableValue.OfType<object>()
                .Select(e => this.collectionType.ElementValueType.ConvertToDocumentValue(e))
                .ToArray();
        }
    }
}