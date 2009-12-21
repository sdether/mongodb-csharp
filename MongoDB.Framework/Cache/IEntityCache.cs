using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Cache
{
    public interface IEntityCache : IDisposable
    {
        void Clear();

        void Remove(object entity);

        void RemoveAllInstancesOf(Type type);

        void Store(string id, object entity);

        bool TryToFind(string id, out object entity);
    }
}