using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Auto
{
    public class MemberOverrides
    {
        public bool Exclude { get; set; }

        public bool IsReference { get; set; }

        public string Key { get; set; }
    }
}