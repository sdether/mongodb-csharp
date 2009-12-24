using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class ComponentValueMap : ValueMap
    {
        /// <summary>
        /// Gets or sets the component class map.
        /// </summary>
        /// <value>The component class map.</value>
        public ComponentClassMap ComponentClassMap { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentValueMap"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="memberType">Type of the member.</param>
        /// <param name="memberGetter">The member getter.</param>
        /// <param name="memberSetter">The member setter.</param>
        /// <param name="componentClassMap">The component class map.</param>
        public ComponentValueMap(string key, string memberName, Type memberType, Func<object, object> memberGetter, Action<object, object> memberSetter, ComponentClassMap componentClassMap)
            : base(key, memberName, memberType, memberGetter, memberSetter)
        {
            if (componentClassMap == null)
                throw new ArgumentNullException("componentClassMap");

            this.ComponentClassMap = componentClassMap;
        }
    }
}