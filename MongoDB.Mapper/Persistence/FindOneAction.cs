using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Mapper.Configuration;
using MongoDB.Mapper.Configuration.Mapping;
using MongoDB.Mapper.Tracking;

namespace MongoDB.Mapper.Persistence
{
    public class FindOneAction : FindActionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindOneAction"/> class.
        /// </summary>
        /// <param name="mongoSession">The mongoSession.</param>
        /// <param name="mongoSessionCache">The mongo session cache.</param>
        /// <param name="changeTracker">The change tracker.</param>
        public FindOneAction(IMongoSessionImplementor mongoSession, IMongoSessionCache mongoSessionCache, IChangeTracker changeTracker)
            : base(mongoSession, mongoSessionCache, changeTracker)
        { }

        /// <summary>
        /// Finds the one.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public object FindOne(Type type, Document conditions)
        {
            var classMap = this.MongoSession.MappingStore.GetClassMapFor(type);
            return this.Find(classMap, conditions, 1, 0, null, null).SingleOrDefault();
        }
    }
}