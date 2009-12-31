using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping
{
    public class MemberMap : Map
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; private set; }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        /// <value>The name of the member.</value>
        public string MemberName { get; private set; }

        /// <summary>
        /// Gets the member getter.
        /// </summary>
        /// <value>The member getter.</value>
        public Func<object, object> MemberGetter { get; private set; }

        /// <summary>
        /// Gets the member setter.
        /// </summary>
        /// <value>The member setter.</value>
        public Action<object, object> MemberSetter { get; private set; }

        /// <summary>
        /// Gets the type of the value.
        /// </summary>
        /// <value>The type of the value.</value>
        public IValueType ValueType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberMap"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="memberGetter">The member getter.</param>
        /// <param name="memberSetter">The member setter.</param>
        /// <param name="valueType">Type of the value.</param>
        public MemberMap(string key, string memberName, Func<object, object> memberGetter, Action<object, object> memberSetter, IValueType valueType)
        {
            if (key == null)
                throw new ArgumentException("Cannot be null or empty.", "key");
            if (memberName == null)
                throw new ArgumentException("Cannot be null or empty.", "memberName");
            if (memberGetter == null)
                throw new ArgumentNullException("memberGetter");
            if (memberSetter == null)
                throw new ArgumentNullException("memberSetter");
            if (valueType == null)
                throw new ArgumentNullException("valueType");

            this.Key = key;
            this.MemberName = memberName;
            this.MemberGetter = memberGetter;
            this.MemberSetter = memberSetter;
            this.ValueType = valueType;
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IMapVisitor visitor)
        {
            visitor.ProcessMember(this);
        }

        /// <summary>
        /// Maps the member from a document.
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        public virtual void MapFromDocument(IMappingContext mappingContext)
        {
            if (mappingContext == null)
                throw new ArgumentNullException("mappingContext");
            var value = mappingContext.Document[this.Key];
            value = this.ValueType.ConvertFromDocumentValue(value, mappingContext);
            this.MemberSetter(mappingContext.Entity, value);
        }

        /// <summary>
        /// Maps the member to the document.
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        public virtual void MapToDocument(IMappingContext mappingContext)
        {
            if (mappingContext == null)
                throw new ArgumentNullException("mappingContext");
            var value = this.MemberGetter(mappingContext.Entity);
            value = this.ValueType.ConvertToDocumentValue(value, mappingContext);
            mappingContext.Document[this.Key] = value;
        }
    }
}