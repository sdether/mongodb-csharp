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
        protected IChangeTracker ChangeTracker { get; private set; }
        protected IMongoSessionCache MongoSessionCache { get; private set; }
        protected IMongoSessionImplementor MongoSession{ get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistenceAction"/> class.
        /// </summary>
        /// <param name="mongoSession">The mongo session.</param>
        /// <param name="mongoSessionCache">The mongo session cache.</param>
        /// <param name="changeTracker">The change tracker.</param>
        public PersistenceAction(IMongoSessionImplementor mongoSession, IMongoSessionCache mongoSessionCache, IChangeTracker changeTracker)
        {
            if (mongoSession == null)
                throw new ArgumentNullException("mongoSession");
            if (mongoSessionCache == null)
                throw new ArgumentNullException("mongoSessionCache");
            if (changeTracker == null)
                throw new ArgumentNullException("changeTracker");

            this.ChangeTracker = changeTracker;
            this.MongoSessionCache = mongoSessionCache;
            this.MongoSession = mongoSession;
        }

        protected IMongoCollection GetCollectionForClassMap(ClassMap classMap)
        {
            var db = this.MongoSession.Database;
            return db.GetCollection(classMap.CollectionName);
        }
    }
}