using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Mapping.Models
{
    public class EmbeddedValuePart
    {
        public IValueType CustomValueType { get; set; }
    }
}