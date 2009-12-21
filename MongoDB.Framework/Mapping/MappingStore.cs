using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public abstract class MappingStore
    {
        private Dictionary<Type, DocumentMap> documentMaps;

        public MappingStore()
        {
            this.documentMaps = new Dictionary<Type, DocumentMap>();
        }

        public void AddCollectionMap(CollectionMap collectionMap)
        {
            if (collectionMap == null)
                throw new ArgumentNullException("collectionMap");

            this.documentMaps.Add(collectionMap.EntityType, collectionMap);
            foreach (var subDocumentMap in collectionMap.SubDocumentMaps)
                this.documentMaps.Add(subDocumentMap.EntityType, subDocumentMap);
        }

        public DocumentMap GetDocumentMapFor<TEntity>()
        {
            return this.GetDocumentMapFor(typeof(TEntity));
        }

        public DocumentMap GetDocumentMapFor(Type entityType)
        {
            DocumentMap documentMap = null;
            if(!this.documentMaps.TryGetValue(entityType, out documentMap))
            {
                if (!this.TryGetMissingDocumentMap(entityType, out documentMap))
                    throw new UnmappedTypeException(string.Format("The type {0} is unmapped.", entityType));
            }

            return documentMap;
        }

        /// <summary>
        /// Tries to get the missing document map.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="documentMap">The document map.</param>
        /// <returns></returns>
        protected abstract bool TryGetMissingDocumentMap(Type entityType, out DocumentMap documentMap);
    }
}