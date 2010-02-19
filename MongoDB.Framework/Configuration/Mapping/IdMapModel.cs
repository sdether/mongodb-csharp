using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.Mapping;

namespace MongoDB.Framework.Configuration.Mapping
{
    public class IdMapModel : ConvertibleMemberMapModel
    {
        /// <summary>
        /// Gets or sets the generator.
        /// </summary>
        /// <value>The generator.</value>
        public IIdGenerator Generator { get; set; }

        /// <summary>
        /// Gets or sets the unsaved value.
        /// </summary>
        /// <value>The unsaved value.</value>
        public object UnsavedValue { get; set; }
    }
}