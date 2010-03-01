using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping
{
    public class ReferenceMapModel : PersistentMemberMapModel
    {
        public bool IsLazy { get; set; }

        public ReferenceMapModel()
        { }
    }
}