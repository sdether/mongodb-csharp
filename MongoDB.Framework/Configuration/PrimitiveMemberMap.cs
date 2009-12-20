using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using MongoDB.Driver;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Configuration
{
    public class PrimitiveMemberMap : MemberMap
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the converter.
        /// </summary>
        /// <value>The converter.</value>
        public IValueConverter Converter { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberMap"/> class.
        /// </summary>
        /// <param name="memberInfo">The member info.</param>
        public PrimitiveMemberMap(string memberName, Func<object, object> getter, Action<object, object> setter)
            : base(memberName, getter, setter)
        {
            this.Converter = NoOpValueConverter.Instance;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberMap"/> class.
        /// </summary>
        /// <param name="memberInfo">The member info.</param>
        /// <param name="documentKey">The document key.</param>
        public PrimitiveMemberMap(string memberName, Func<object, object> getter, Action<object, object> setter, string documentKey)
            : base(memberName, getter, setter, documentKey)
        {
            this.Converter = NoOpValueConverter.Instance;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IMapVisitor visitor)
        {
            visitor.VisitPrimitiveMemberMap(this);
        }

        /// <summary>
        /// Gets the value from document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public override object GetValueFromDocument(Document document)
        {
            var value = document[this.DocumentKey];
            if (value == MongoDBNull.Value)
                value = null;
            return this.Converter.ConvertFromDocumentValue(value);
        }

        /// <summary>
        /// Sets the value on document.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="document">The document.</param>
        public override void SetValueOnDocument(object value, Document document)
        {
            value = this.Converter.ConvertToDocumentValue(value);
            document[this.DocumentKey] = value ?? MongoDBNull.Value;
        }

        #endregion
    }
}