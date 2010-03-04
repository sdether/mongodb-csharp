﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Mapping.Conventions
{
    public interface IIdConvention
    {
        bool IsId(MemberInfo memberInfo);
    }
}