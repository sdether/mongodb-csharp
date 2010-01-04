using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping
{
    public class ManyToOneMap : MemberMapBase
    {
        //public bool IsLazy { get; private set; }

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
        /// <param name="referenceType">Type of the reference.</param>
        /// <param name="cascade">The cascade.</param>
        public ManyToOneMap(string key, string memberName, Func<object, object> memberGetter, Action<object, object> memberSetter, Type referenceType)
            : base(key, memberName, memberGetter, memberSetter)
        {
            if (referenceType == null)
                throw new ArgumentNullException("referenceType");

            this.ReferenceType = referenceType;
        }

        public override void Accept(IMapVisitor visitor)
        {
            visitor.ProcessManyToOne(this);
        }
    }
}