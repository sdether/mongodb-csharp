using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper.Configuration.Mapping
{
    public class SubClassMapModel : ClassMapModelBase
    {
        public SubClassMapModel(Type type)
            : base(type)
        { }
    }
}