using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public interface IDiscriminatorConvention : IConvention<Type>
    {
        string GetDiscriminatorKey(Type type);

        object GetDiscriminator(Type type);

        bool HasDiscriminator(Type type);
    }
}