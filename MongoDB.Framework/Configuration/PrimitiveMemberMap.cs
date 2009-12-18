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
        /// Gets the document value from entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override object GetDocumentValueFromEntity(object entity)
        {
            return this.Converter.ConvertToDocumentValue(
                this.Getter(entity));
        }

        /// <summary>
        /// Sets the document value on entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="documentValue">The document value.</param>
        public override void SetDocumentValueOnEntity(object entity, object documentValue)
        {
            this.Setter(
                entity, 
                this.Converter.ConvertFromDocumentValue(documentValue));
        }

        #endregion
    }
}