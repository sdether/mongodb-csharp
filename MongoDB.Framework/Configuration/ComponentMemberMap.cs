using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using MongoDB.Framework.Reflection;
using MongoDB.Driver;

namespace MongoDB.Framework.Configuration
{
    public class ComponentMemberMap : MemberMap
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the entity map.
        /// </summary>
        /// <value>The entity map.</value>
        public EntityMap EntityMap { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentMap"/> class.
        /// </summary>
        /// <param name="memberInfo">The member info.</param>
        /// <param name="entityMap">The entity map.</param>
        public ComponentMemberMap(string memberName, Func<object, object> getter, Action<object, object> setter, EntityMap entityMap)
            : base(memberName, getter, setter)
        {
            if (entityMap == null)
                throw new ArgumentNullException("entityMap");

            this.EntityMap = entityMap;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMemberMap"/> class.
        /// </summary>
        /// <param name="memberInfo">The member info.</param>
        /// <param name="documentKey">The document key.</param>
        /// <param name="entityMap">The entity map.</param>
        public ComponentMemberMap(string memberName, Func<object, object> getter, Action<object, object> setter, string documentKey, EntityMap entityMap)
            : base(memberName, getter, setter, documentKey)
        {
            if (entityMap == null)
                throw new ArgumentNullException("entityMap");

            this.EntityMap = entityMap;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IMapVisitor visitor)
        {
            visitor.VisitComponentMemberMap(this);
        }

        /// <summary>
        /// Gets the value from document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public override object GetValueFromDocument(Document document)
        {
            return document[this.DocumentKey];
        }

        /// <summary>
        /// Sets the value on document.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="document">The document.</param>
        public override void SetValueOnDocument(object value, Document document)
        {
            document[this.DocumentKey] = value;
        }

        #endregion
    }
}