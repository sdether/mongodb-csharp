using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper.Mapping
{
    public abstract class PersistentMemberMap : MemberMap
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; private set; }

        /// <summary>
        /// Gets a value indicating whether null should be persisted.
        /// </summary>
        /// <value><c>true</c> if [persist null]; otherwise, <c>false</c>.</value>
        public bool PersistNull { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistentMemberMapBase"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="memberGetter">The member getter.</param>
        /// <param name="memberSetter">The member setter.</param>
        /// <param name="persistNull">if set to <c>true</c> [persist null].</param>
        protected PersistentMemberMap(string key, string memberName, Func<object, object> memberGetter, Action<object, object> memberSetter, bool persistNull)
            : base(memberName, memberGetter, memberSetter)
        {
            if (key == null)
                throw new ArgumentException("Cannot be null or empty.", "key");

            this.Key = key;
            this.PersistNull = persistNull;
        }
    }
}