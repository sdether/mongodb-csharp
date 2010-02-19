using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public interface IExtendedPropertiesConvention : IConvention<Type>
    {
        ExtendedPropertiesMapModel GetExtendedPropertiesMapModel(Type type);

        bool HasExtendedProperties(Type type);
    }
}