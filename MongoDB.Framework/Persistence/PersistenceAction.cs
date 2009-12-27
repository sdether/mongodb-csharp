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
        protected IMongoContext MongoContext{ get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistenceAction"/> class.
        /// </summary>
        /// <param name="mongoContext">The mongo context.</param>
        /// <param name="changeTracker">The change tracker.</param>
        /// <param name="collection">The collection.</param>
        public PersistenceAction(IMongoContext mongoContext, ChangeTracker changeTracker)
        {
            if (mongoContext == null)
                throw new ArgumentNullException("mongoContext");
            if (changeTracker == null)
                throw new ArgumentNullException("changeTracker");

            this.ChangeTracker = changeTracker;
            this.MongoContext = mongoContext;
        }

        protected IMongoCollection GetCollectionForClassMap(ClassMap classMap)
        {
            var db = this.MongoContext.Database;
            return db.GetCollection(classMap.CollectionName);
        }
    }
}