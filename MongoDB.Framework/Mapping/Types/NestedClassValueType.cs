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
        public override object ConvertFromDocumentValue(object documentValue, IMappingContext mappingContext)
        {
            documentValue = base.ConvertFromDocumentValue(documentValue, mappingContext);
            var document = documentValue as Document;
            if(document == null)
                return null;

            ClassMap concreteClassMap = this.NestedClassMap;
            if (this.NestedClassMap.IsPolymorphic)
            {
                object discriminator = document[concreteClassMap.DiscriminatorKey];
                concreteClassMap = concreteClassMap.GetClassMapByDiscriminator(discriminator);
            }

            var entity = Activator.CreateInstance(concreteClassMap.Type);
            var childContext = mappingContext.CreateChildMappingContext(document, entity);
            this.NestedClassMap.MapFromDocument(childContext);
            return childContext.Entity;
        }

        /// <summary>
        /// Converts to document value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns></returns>
        public override object ConvertToDocumentValue(object value, IMappingContext mappingContext)
        {
            value = base.ConvertToDocumentValue(value, mappingContext);
            if (value == MongoDBNull.Value)
                return value;

            var document = new Document();
            mappingContext = mappingContext.CreateChildMappingContext(document, value);
            this.NestedClassMap.MapToDocument(mappingContext);
            return document;
        }
    }
}