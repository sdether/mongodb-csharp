using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public interface IConvention<T>
    {
        bool Matches(T topic);
    }
}
