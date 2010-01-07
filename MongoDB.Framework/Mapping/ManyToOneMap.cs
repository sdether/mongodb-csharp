using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class ManyToOneMap : MemberMapBase
    {
        public bool IsLazy { get; private set; }

        /// <summary>
        /// Gets or sets the type of the reference.
        /// </summary>
        /// <value>The type of the reference.</value>
        public Type ReferenceType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManyToOneMap"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="memberGetter">The member getter.</param>
        /// <param name="memberSetter">The member setter.</param>
        /// <param name="persistNull">if set to <c>true</c> [persist nulls].</param>
        /// <param name="referenceType">Type of the reference.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        public ManyToOneMap(string key, string memberName, Func<object, object> memberGetter, Action<object, object> memberSetter, bool persistNull, Type referenceType, bool isLazy)
            : base(key, memberName, memberGetter, memberSetter, persistNull)
        {
            if (referenceType == null)
                throw new ArgumentNullException("referenceType");

            this.IsLazy = isLazy;
            this.ReferenceType = referenceType;
        }

        public override void Accept(IMapVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}