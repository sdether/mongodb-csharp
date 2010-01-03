﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Mapping.Models
{
    public class MemberMapModel : MemberMapModelBase
    {
        public EmbeddedMemberPart Part { get; set; }

        public override void Accept(IMapModelVisitor visitor)
        {
            visitor.ProcessMember(this);
        }
    }
}