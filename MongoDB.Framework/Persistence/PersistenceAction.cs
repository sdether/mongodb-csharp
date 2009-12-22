using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Tracking;

namespace MongoDB.Framework.Persistence
{
    public abstract class PersistenceAction
    {
        protected ChangeTracker ChangeTracker { get; private set; }
        protected IMongoCollection Collection { get; private set; }
        protected MappingStore MappingStore { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistenceAction"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="changeTracker">The change tracker.</param>
        /// <param name="collection">The collection.</param>
        public PersistenceAction(MappingStore mappingStore, ChangeTracker changeTracker, IMongoCollection collection)
        {
            if (mappingStore == null)
                throw new ArgumentNullException("mappingStore");
            if (changeTracker == null)
                throw new ArgumentNullException("changeTracker");
            if (collection == null)
                throw new ArgumentNullException("collection");

            this.ChangeTracker = changeTracker;
            this.Collection = collection;
            this.MappingStore = mappingStore;
        }
    }
}