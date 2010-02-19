using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping
{
    public class NestedClassMapModel : SuperClassMapModel
    {
        /// <summary>
        /// Gets or sets the parent map.
        /// </summary>
        /// <value>The parent map.</value>
        public ParentMemberMapModel ParentMap { get; set; }

        public NestedClassMapModel(Type type)
            : base(type)
        { }

    }
}
