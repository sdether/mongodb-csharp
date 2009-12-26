using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping.Types
{
    public class NestedClassValueType : NullSafeValueType
    {
        /// <summary>
        /// Gets or sets the nested class map.
        /// </summary>
        /// <value>The nested class map.</value>
        public NestedClassMap NestedClassMap { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NestedClassValueType"/> class.
        /// </summary>
        /// <param name="nestedClassMap">The nested class map.</param>
        public NestedClassValueType(NestedClassMap nestedClassMap)
            : base(nestedClassMap.Type)
        {
            this.NestedClassMap = nestedClassMap;
        }

        /// <summary>
        /// Converts from document value.
        /// </summary>
        /// <param name="documentValue">The document value.</param>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns></returns>
        public override object ConvertFromDocumentValue(object documentValue, MappingContext mappingContext)
        {
            documentValue = base.ConvertFromDocumentValue(documentValue, mappingContext);
            var document = documentValue as Document;
            if(document == null)
                return null;

            var childContext = mappingContext.CreateChildMappingContext(document, this.NestedClassMap.Type);
            this.NestedClassMap.Map(childContext);
            return childContext.Entity;
        }

        public override object ConvertToDocumentValue(object value, MappingContext mappingContext)
        {
            value = base.ConvertToDocumentValue(value, mappingContext);
            if (value == MongoDBNull.Value)
                return value;

            var childContext = mappingContext.CreateChildMappingContext(value);
            this.NestedClassMap.Map(childContext);
            return childContext.Document;
        }
    }
}