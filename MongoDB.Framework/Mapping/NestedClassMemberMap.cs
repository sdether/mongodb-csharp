using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class NestedClassMemberMap : MemberMap
    {
        /// <summary>
        /// Gets or sets the nestedClass class map.
        /// </summary>
        /// <value>The nestedClass class map.</value>
        public NestedClassMap NestedClassMap { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NestedClassMemberMap"/> class.
        /// </summary>
        /// <param name="nestedClassMap">The nestedClass class map.</param>
        public NestedClassMemberMap(NestedClassMap nestedClassMap)
        {
            if (nestedClassMap == null)
                throw new ArgumentNullException("nestedClassMap");

            this.NestedClassMap = nestedClassMap;
        }
    }
}