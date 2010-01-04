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
    public class FindAction : FindActionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindAction"/> class.
        /// </summary>
        /// <param name="mongoContext">The mongoContext.</param>
        /// <param name="mongoContextCache">The mongo context cache.</param>
        public FindAction(IMongoContext mongoContext, IMongoContextCache mongoContextCache)
            : base(mongoContext, mongoContextCache)
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
            var classMap = this.MongoContext.MappingStore.GetClassMapFor(type);
            return this.Find(classMap, conditions, limit, skip, orderBy, fields);
        }

    }
}