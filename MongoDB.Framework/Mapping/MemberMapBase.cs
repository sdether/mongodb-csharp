using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public abstract class MemberMapBase : Map
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
        /// Initializes a new instance of the <see cref="MemberMapBase"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="memberGetter">The member getter.</param>
        /// <param name="memberSetter">The member setter.</param>
        protected MemberMapBase(string key, string memberName, Func<object, object> memberGetter, Action<object, object> memberSetter)
        {
            if (key == null)
                throw new ArgumentException("Cannot be null or empty.", "key");
            if (memberName == null)
                throw new ArgumentException("Cannot be null or empty.", "memberName");
            if (memberGetter == null)
                throw new ArgumentNullException("memberGetter");
            if (memberSetter == null)
                throw new ArgumentNullException("memberSetter");

            this.Key = key;
            this.MemberName = memberName;
            this.MemberGetter = memberGetter;
            this.MemberSetter = memberSetter;
        }
    }
}