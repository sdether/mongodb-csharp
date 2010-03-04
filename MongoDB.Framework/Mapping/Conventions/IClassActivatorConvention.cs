using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Conventions
{
    public interface IClassActivatorConvention
    {
        IClassActivator GetClassActivator(Type type);
    }
}