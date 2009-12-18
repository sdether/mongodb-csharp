using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using MongoDB.Driver;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Configuration
{
    public class MemberMap : IMapVisitable
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the converter.
        /// </summary>
        /// <value>The converter.</value>
        public IValueConverter Converter { get; set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public string DocumentKey { get; private set; }

        /// <summary>
        /// Gets or sets the getter.
        /// </summary>
        /// <value>The getter.</value>
        public Func<object, object> Getter { get; private set; }

        /// <summary>
        /// Gets or sets the name of the member.
        /// </summary>
        /// <value>The name of the member.</value>
        public string MemberName { get; private set; }

        /// <summary>
        /// Gets or sets the setter.
        /// </summary>
        /// <value>The setter.</value>
        public Action<object, object> Setter { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberMap"/> class.
        /// </summary>
        /// <param name="memberInfo">The member info.</param>
        public MemberMap(MemberInfo memberInfo)
        {
            if (memberInfo == null)
                throw new ArgumentNullException("memberInfo");

            this.Converter = DefaultValueConverter.Instance;
            this.DocumentKey = memberInfo.Name;
            this.MemberName = memberInfo.Name;
            this.Getter = LateBoundReflection.GetGetter(memberInfo);
            this.Setter = LateBoundReflection.GetSetter(memberInfo);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberMap"/> class.
        /// </summary>
        /// <param name="memberInfo">The member info.</param>
        /// <param name="documentKey">The document key.</param>
        public MemberMap(MemberInfo memberInfo, string documentKey)
            : this(memberInfo)
        {
            if (documentKey == null)
                throw new ArgumentException("Cannot be null or empty.", "documentKey");

            this.DocumentKey = documentKey;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public virtual void Accept(IMapVisitor visitor)
        {
            visitor.VisitMemberMap(this);
        }

        /// <summary>
        /// Gets the document value from entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public object GetDocumentValueFromEntity(object entity)
        {
            return this.Converter.ConvertToDocumentValue(
                this.Getter(entity));
        }

        /// <summary>
        /// Sets the document value on entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="documentValue">The document value.</param>
        public void SetDocumentValueOnEntity(object entity, object documentValue)
        {
            this.Setter(
                entity, 
                this.Converter.ConvertFromDocumentValue(documentValue));
        }

        #endregion
    }
}