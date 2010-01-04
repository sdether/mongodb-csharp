using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Models
{
    public class EmbeddedValuePart : EmbeddedMemberPart
    {
        public IValueType CustomValueType { get; set; }
    }
}