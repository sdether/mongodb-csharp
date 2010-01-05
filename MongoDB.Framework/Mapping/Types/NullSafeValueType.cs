﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping.Types
{
    public class NullSafeValueType : IValueType
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullSafeValueType"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public NullSafeValueType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            this.Type = type;
        }

        /// <summary>
        /// Converts from document value.
        /// </summary>
        /// <param name="documentValue">The document value.</param>
        /// <param name="mongoContext">The mongo context.</param>
        /// <returns></returns>
        public virtual object ConvertFromDocumentValue(object documentValue, IMongoContextImplementor mongoContext)
        {
            if (documentValue == null || documentValue == MongoDBNull.Value)
                return this.Type.IsValueType ? Activator.CreateInstance(this.Type) : null;

            return documentValue;
        }

        /// <summary>
        /// Converts to document value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="mongoContext">The mongo context.</param>
        /// <returns></returns>
        public virtual object ConvertToDocumentValue(object value, IMongoContextImplementor mongoContext)
        {
            if (value == null)
                return MongoDBNull.Value;

            return value;
        }
    }
}