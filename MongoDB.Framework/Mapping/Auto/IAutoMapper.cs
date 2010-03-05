﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Auto
{
    public interface IAutoMapper
    {
        bool CanCreateClassMap(Type type);

        ClassMapBase CreateClassMap(Type type, Func<Type, ClassMapBase> existingClassMapFinder);
    }
}