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
        { }

        /// <summary>
        /// Converts from document value.
        /// </summary>
        /// <param name="documentValue">The document value.</param>
        /// <param name="translationContext">The translation context.</param>
        /// <returns></returns>
        public override object ConvertFromDocumentValue(object documentValue, TranslationContext translationContext)
        {
            documentValue = base.ConvertFromDocumentValue(documentValue, translationContext);
            var document = documentValue as Document;
            if(document == null)
                return null;

            var childContext = translationContext.CreateChild();
            childContext.Document = document;
            this.NestedClassMap.TranslateFromDocument(childContext);
            return childContext.Owner;
        }

        public override object ConvertToDocumentValue(object value, TranslationContext translationContext)
        {
            throw new NotImplementedException();
        }
    }
}