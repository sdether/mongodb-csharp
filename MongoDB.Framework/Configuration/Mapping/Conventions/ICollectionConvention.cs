using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MongoDB.Framework.Mapping;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public interface ICollectionConvention : IConvention<Type>
    {
        ICollectionType GetCollectionType(Type type);

        Type GetElementType(Type type);

        bool IsCollection(Type type);
    }
}