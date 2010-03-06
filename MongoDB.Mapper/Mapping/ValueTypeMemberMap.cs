using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Mapper.Mapping
{
    public class ValueTypeMemberMap : PersistentMemberMap
    {
        /// <summary>
        /// Gets the type of the value.
        /// </summary>
        /// <value>The type of the value.</value>
        public ValueTypeBase ValueType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberMap"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="memberGetter">The member getter.</param>
        /// <param name="memberSetter">The member setter.</param>
        /// <param name="persistNull">if set to <c>true</c> [persist nulls].</param>
        /// <param name="valueType">Type of the value.</param>
        public ValueTypeMemberMap(string key, string memberName, Func<object, object> memberGetter, Action<object, object> memberSetter, bool persistNull, ValueTypeBase valueType)
            : base(key, memberName, memberGetter, memberSetter, persistNull)
        {
            if (valueType == null)
                throw new ArgumentNullException("valueType");
            
            this.ValueType = valueType;
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IMapVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}