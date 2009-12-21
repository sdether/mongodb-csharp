using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Hydration;
using MongoDB.Framework.Tracking;

namespace MongoDB.Framework.Persistence
{
    public abstract class PersistenceAction
    {
        protected ChangeTracker ChangeTracker { get; private set; }
        protected IMongoCollection Collection { get; private set; }
        protected IEntityHydrator Hydrator { get; private set; }
        protected MappingStore MappingStore { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistenceAction"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="changeTracker">The change tracker.</param>
        /// <param name="hydrator">The hydrator.</param>
        /// <param name="mongoCollection">The mongo collection.</param>
        public PersistenceAction(MappingStore mappingStore, ChangeTracker changeTracker, IEntityHydrator hydrator, IMongoCollection mongoCollection)
        {
            if (mappingStore == null)
                throw new ArgumentNullException("mappingStore");
            if (changeTracker == null)
                throw new ArgumentNullException("changeTracker");
            if (hydrator == null)
                throw new ArgumentNullException("hydrator");
            if (mongoCollection == null)
                throw new ArgumentNullException("mongoCollection");

            this.ChangeTracker = changeTracker;
            this.Collection = Collection;
            this.Hydrator = hydrator;
            this.MappingStore = mappingStore;
        }
    }
}