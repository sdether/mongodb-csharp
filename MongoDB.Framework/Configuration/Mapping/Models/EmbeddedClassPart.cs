using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Models
{
    public class EmbeddedClassPart : EmbeddedMemberPart
    {
        public NestedClassMapModel NestedClassMap { get; set; }
    }
}