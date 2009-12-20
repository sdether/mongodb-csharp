using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MongoDB.Framework.Reflection;
using MongoDB.Driver;

namespace MongoDB.Framework.Configuration
{
    public abstract class MemberMap : IMapVisitable
    {
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

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberMap"/> class.
        /// </summary>
        /// <param name="memberInfo">The member info.</param>
        protected MemberMap(string memberName, Func<object, object> getter, Action<object, object> setter)
        {
            if (memberName == null)
                throw new ArgumentException("Cannot be null or empty.", "memberName");
            if (getter == null)
                throw new ArgumentNullException("getter");
            if (setter == null)
                throw new ArgumentNullException("setter");

            this.DocumentKey = memberName;
            this.MemberName = memberName;
            this.Getter = getter;
            this.Setter = setter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberMap"/> class.
        /// </summary>
        /// <param name="memberInfo">The member info.</param>
        /// <param name="documentKey">The document key.</param>
        protected MemberMap(string memberName, Func<object, object> getter, Action<object, object> setter, string documentKey)
            : this(memberName, getter, setter)
        {
            if (documentKey == null)
                throw new ArgumentException("Cannot be null or empty.", "documentKey");

            this.DocumentKey = documentKey;
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public abstract void Accept(IMapVisitor visitor);

        /// <summary>
        /// Gets the value from document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public abstract object GetValueFromDocument(Document document);

        /// <summary>
        /// Sets the value on document.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="document">The document.</param>
        public abstract void SetValueOnDocument(object value, Document document);
    }
}