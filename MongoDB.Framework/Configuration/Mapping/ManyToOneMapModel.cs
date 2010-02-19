using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping
{
    public class ManyToOneMapModel : PersistentMemberMapModel
    {
        public bool? IsLazy { get; set; }

        public ManyToOneMapModel()
        { }
    }
}