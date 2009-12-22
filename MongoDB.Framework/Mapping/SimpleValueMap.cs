using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace MongoDB.Framework.Mapping
{
    public class SimpleValueMap : ValueMap
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleValueMap"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="memberType">Type of the member.</param>
        /// <param name="memberGetter">The member getter.</param>
        /// <param name="memberSetter">The member setter.</param>
        public SimpleValueMap(string key, string memberName, Type memberType, Func<object, object> memberGetter, Action<object, object> memberSetter)
            : base(key, memberName, memberType, memberGetter, memberSetter)
        { }

        #endregion
    }
}