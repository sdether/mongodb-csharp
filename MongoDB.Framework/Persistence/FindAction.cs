using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Tracking;

namespace MongoDB.Framework.Persistence
{
    public class FindAction : FindActionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindAction"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="changeTracker">The change tracker.</param>
        /// <param name="collection">The collection.</param>
        public FindAction(MappingStore mappingStore, ChangeTracker changeTracker, IMongoCollection collection)
            : base(mappingStore, changeTracker, collection)
        { }

        /// <summary>
        /// Finds the specified query.
        /// </summary>
        /// <param name="type">Type of the entity.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="conditions">The conditions.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns></returns>
        public IEnumerable<object> Find(Type type, Document conditions, int limit, int skip, Document orderBy, Document fields)
        {
            var classMap = this.MappingStore.GetClassMapFor(type);
            return this.Find(classMap, conditions, limit, skip, orderBy, fields);
        }

    }
}