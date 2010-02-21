using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping.Auto
{
    public interface ITypeSource
    {
        IEnumerable<Type> GetTypes();
    }
}