using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class MappingStore
    {
        public MappingStore()
        {
            throw new NotImplementedException();
        }

        public DocumentMap GetDocumentMapFor<TEntity>()
        {
            throw new NotImplementedException();
        }

        public DocumentMap GetDocumentMapFor(Type entityType)
        {
            throw new NotImplementedException();
        }

        public CollectionMap GetCollectionMapFor(Type entityType)
        {
            throw new NotImplementedException();
        }

    }
}
