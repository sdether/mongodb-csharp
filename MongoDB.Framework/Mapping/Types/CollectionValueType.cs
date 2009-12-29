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
        private IValueType elementValueType;

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type
        {
            get { return collectionType.GetCollectionType(this.elementValueType); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionValueType"/> class.
        /// </summary>
        /// <param name="collectionType">Type of the collection.</param>
        /// <param name="elementValueType">Type of the element value.</param>
        public CollectionValueType(ICollectionType collectionType, IValueType elementValueType)
        {
            if (collectionType == null)
                throw new ArgumentNullException("collectionType");
            if (elementValueType == null)
                throw new ArgumentNullException("elementValueType");

            this.collectionType = collectionType;
            this.elementValueType = elementValueType;
        }

        /// <summary>
        /// Converts from document value.
        /// </summary>
        /// <param name="documentValue">The document value.</param>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns></returns>
        public object ConvertFromDocumentValue(object documentValue, MappingContext mappingContext)
        {
            return this.collectionType.ConvertFromDocumentValue(this.elementValueType, documentValue, mappingContext);
        }

        /// <summary>
        /// Converts to document value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public object ConvertToDocumentValue(object value)
        {
            return this.collectionType.ConvertToDocumentValue(this.elementValueType, value);
        }
    }
}