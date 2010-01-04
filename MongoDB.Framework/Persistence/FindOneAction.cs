using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;
using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Tracking;

namespace MongoDB.Framework.Persistence
{
    public class FindOneAction : FindActionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindOneAction"/> class.
        /// </summary>
        /// <param name="mongoContext">The mongoContext.</param>
        /// <param name="mongoContextCache">The mongo context cache.</param>
        /// <param name="changeTracker">The change tracker.</param>
        public FindOneAction(IMongoContext mongoContext, IMongoContextCache mongoContextCache, IChangeTracker changeTracker)
            : base(mongoContext, mongoContextCache, changeTracker)
        { }

        /// <summary>
        /// Finds the one.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public object FindOne(Type type, Document conditions)
        {
            var classMap = this.MongoContext.MappingStore.GetClassMapFor(type);
            return this.Find(classMap, conditions, 1, 0, null, null).SingleOrDefault();
        }
    }
}