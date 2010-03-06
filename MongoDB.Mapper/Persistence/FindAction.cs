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
    public class FindAction : FindActionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindAction"/> class.
        /// </summary>
        /// <param name="mongoSession">The mongoSession.</param>
        /// <param name="mongoSessionCache">The mongo session cache.</param>
        /// <param name="changeTracker">The change tracker.</param>
        public FindAction(IMongoSessionImplementor mongoSession, IMongoSessionCache mongoSessionCache, IChangeTracker changeTracker)
            : base(mongoSession, mongoSessionCache, changeTracker)
        { }

        /// <summary>
        /// Finds the specified query.
        /// </summary>
        /// <param name="type">ValueType of the entity.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="conditions">The conditions.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns></returns>
        public IEnumerable<object> Find(Type type, Document conditions, int limit, int skip, Document orderBy, Document fields)
        {
            var classMap = this.MongoSession.MappingStore.GetClassMapFor(type);
            return this.Find(classMap, conditions, limit, skip, orderBy, fields);
        }

    }
}