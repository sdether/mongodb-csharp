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
        /// <param name="componentClassMap">The component class map.</param>
        public ComponentValueMap(ComponentClassMap componentClassMap)
        {
            if (componentClassMap == null)
                throw new ArgumentNullException("componentClassMap");

            this.ComponentClassMap = componentClassMap;
        }
    }
}