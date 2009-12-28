using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Model
{
    public class SubClassMapModel : ClassMapModel
    {
        public SubClassMapModel(Type type)
            : base(type)
        { }
    }
}