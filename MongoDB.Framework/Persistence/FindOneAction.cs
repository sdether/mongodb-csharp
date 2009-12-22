using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Tracking;

namespace MongoDB.Framework.Persistence
{
    public class FindOneAction : FindActionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindOneAction"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="changeTracker">The change tracker.</param>
        /// <param name="collection">The collection.</param>
        public FindOneAction(MappingStore mappingStore, ChangeTracker changeTracker, IMongoCollection collection)
            : base(mappingStore, changeTracker, collection)
        { }

        /// <summary>
        /// Finds the one.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public object FindOne(Type entityType, Document query)
        {
            //TODO: analyze the query to see if it only contains an id key so we can check our change tracking...
            var document = this.Collection.FindOne(query);
            return this.CreateEntity(entityType, document);
        }
    }
}