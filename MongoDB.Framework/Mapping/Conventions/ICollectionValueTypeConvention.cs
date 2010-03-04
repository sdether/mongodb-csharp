using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Conventions
{
    public interface ICollectionValueTypeConvention
    {
        bool IsCollection(Type type);

        ICollectionType GetCollectionType(Type type);

        Type GetElementType(Type type);
    }
}