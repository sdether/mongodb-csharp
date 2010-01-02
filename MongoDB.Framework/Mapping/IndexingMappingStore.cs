using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping
{
    public class IndexingMappingStore : IMappingStore
    {
        #region Private Static Fields

        private static readonly HashSet<Type> indexesCreated = new HashSet<Type>();
        private static readonly object indexLock = new object();

        #endregion

        #region Private Fields

        private Database database;
        private IMappingStore mappingStore;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexingMappingStore"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="database">The database.</param>
        public IndexingMappingStore(IMappingStore mappingStore, Database database)
        {
            if (mappingStore == null)
                throw new ArgumentNullException("mappingStore");
            if (database == null)
                throw new ArgumentNullException("database");

            this.database = database;
            this.mappingStore = mappingStore;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the class map for the specified entity type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public ClassMap GetClassMapFor(Type type)
        {
            var classMap = this.mappingStore.GetClassMapFor(type);
            this.CreateIndexesIfNecessary(classMap);
            return classMap;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates the index if necessary.
        /// </summary>
        private void CreateIndexesIfNecessary(ClassMap classMap)
        {
            if (!indexesCreated.Contains(classMap.Type))
            {
                lock (indexLock)
                {
                    if (!indexesCreated.Contains(classMap.Type))
                    {
                        this.CreateIndexes(classMap);
                    }
                }
            }
        }

        private void CreateIndexes(ClassMap classMap)
        {
            RootClassMap rootClassMap = classMap as RootClassMap;
            if (rootClassMap == null)
                rootClassMap = (RootClassMap)((SubClassMap)classMap).SuperClassMap;

            var collectionMetaData = this.database.GetCollection(rootClassMap.CollectionName)
                .MetaData;
            foreach (var index in classMap.Indexes)
            {
                Document fieldsAndDirections = new Document();
                foreach(var part in index.Parts)
                {
                    fieldsAndDirections.Add(
                        part.Key,
                        part.Value == IndexDirection.Ascending ? 1 : -1);
                }

                collectionMetaData.CreateIndex(index.Name, fieldsAndDirections, index.IsUnique);
            }

            indexesCreated.Add(rootClassMap.Type);
            foreach (var subClassMap in rootClassMap.SubClassMaps)
                indexesCreated.Add(subClassMap.Type);
        }

        #endregion
    }
}