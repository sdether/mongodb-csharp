﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Mapping.Visitors;

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
        public override object ConvertFromDocumentValue(object documentValue, IMongoContextImplementor mongoContext)
        {
            documentValue = base.ConvertFromDocumentValue(documentValue, mongoContext);
            var document = documentValue as Document;
            if(document == null)
                return null;

            ClassMap concreteClassMap = this.NestedClassMap;
            if (this.NestedClassMap.IsPolymorphic)
            {
                object discriminator = document[concreteClassMap.DiscriminatorKey];
                concreteClassMap = concreteClassMap.GetClassMapByDiscriminator(discriminator);
            }

            var mapper = new DocumentToEntityMapper(mongoContext);
            return mapper.CreateEntity(concreteClassMap, document);
        }

        /// <summary>
        /// Converts to document value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="mongoContext">The mongo context.</param>
        /// <returns></returns>
        public override object ConvertToDocumentValue(object value, IMongoContextImplementor mongoContext)
        {
            value = base.ConvertToDocumentValue(value, mongoContext);
            if (value == MongoDBNull.Value)
                return value;

            var mapper = new EntityToDocumentMapper(mongoContext);
            return mapper.CreateDocument(this.NestedClassMap, value);
        }
    }
}