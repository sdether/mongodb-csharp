using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public interface IIdConvention : IConvention<Type>
    {
        IdMapModel GetIdMapModel(Type type);

        bool HasId(Type type);
    }
}
