using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Models
{
    public class IdMapModel : MemberMapModel
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

        /// <summary>
        /// Gets or sets the type of the custom value.
        /// </summary>
        /// <value>The type of the custom value.</value>
        public IValueType CustomValueType { get; set; }
    }
}