﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Models
{
    public class KeyValueMemberMapModel : KeyMemberMapModel
    {
        public IValueType CustomValueType { get; set; }
    }
}