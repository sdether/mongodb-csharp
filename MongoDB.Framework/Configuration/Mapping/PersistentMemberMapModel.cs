using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping
{
    public class PersistentMemberMapModel : MemberMapModel
    {
        public string Key { get; set; }

        public bool PersistNull { get; set; }
    }
}