using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Models
{
    public class NestedClassMapModel : SuperClassMapModel
    {
        public NestedClassMapModel(Type type)
            : base(type)
        { }
    }
}
