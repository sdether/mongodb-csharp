using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Model
{
    public class NestedClassMemberMapModel : KeyMemberMapModel
    {
        public NestedClassMapModel NestedClassMapModel { get; set; }
    }
}