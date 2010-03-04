using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Auto
{
    public interface IAutoMapper
    {
        ClassMapBase CreateClassMap(Type type, Func<Type, ClassMapBase> existingClassMapFinder);
    }
}