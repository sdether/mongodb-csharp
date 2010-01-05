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
        protected IMongoContextCache MongoContextCache { get; private set; }
        protected IMongoContextImplementor MongoContext{ get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistenceAction"/> class.
        /// </summary>
        /// <param name="mongoContext">The mongo context.</param>
        /// <param name="mongoContextCache">The mongo context cache.</param>
        /// <param name="changeTracker">The change tracker.</param>
        public PersistenceAction(IMongoContextImplementor mongoContext, IMongoContextCache mongoContextCache, IChangeTracker changeTracker)
        {
            if (mongoContext == null)
                throw new ArgumentNullException("mongoContext");
            if (mongoContextCache == null)
                throw new ArgumentNullException("mongoContextCache");
            if (changeTracker == null)
                throw new ArgumentNullException("changeTracker");

            this.ChangeTracker = changeTracker;
            this.MongoContextCache = mongoContextCache;
            this.MongoContext = mongoContext;
        }

        protected IMongoCollection GetCollectionForClassMap(ClassMap classMap)
        {
            var db = this.MongoContext.Database;
            return db.GetCollection(classMap.CollectionName);
        }
    }
}