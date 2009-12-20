using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MongoDB.Framework.Reflection;
using MongoDB.Driver;

namespace MongoDB.Framework.Configuration
{
    public class IdMap : MemberMap
    {
        #region Public Properties

        /// <summary>
        /// Gets the transient value.
        /// </summary>
        /// <value>The transient value.</value>
        public string[] TransientValues
        {
            get { return new [] { null, string.Empty }; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedPropertiesMap"/> class.
        /// </summary>
        /// <param name="memberInfo">The member info.</param>
        public IdMap(string memberName, Func<object, object> getter, Action<object, object> setter)
            : base(memberName, getter, setter, "_id")
        { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IMapVisitor visitor)
        {
            visitor.VisitIdMap(this);
        }

        /// <summary>
        /// Gets the value from document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public override object GetValueFromDocument(Document document)
        {
            Oid documentValue = document[this.DocumentKey] as Oid;
            if (documentValue == null)
                return null;

            return BitConverter.ToString(documentValue.Value).Replace("-","").ToLower();
        }

        /// <summary>
        /// Sets the value on document.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="document">The document.</param>
        public override void SetValueOnDocument(object value, Document document)
        {
            var stringValue = value as string;
            if (value == null)
                return;

            document[this.DocumentKey] = new Oid(stringValue);
        }

        #endregion
    }
}