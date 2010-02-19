using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public interface ICollectionNameConvention : IConvention<Type>
    {
        string GetCollectionName(Type type);
    }
}