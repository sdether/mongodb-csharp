using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Tracking;

namespace MongoDB.Framework.Persistence
{
    public class GetByIdAction : FindActionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdAction"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="changeTracker">The change tracker.</param>
        /// <param name="collection">The collection.</param>
        public GetByIdAction(MappingStore mappingStore, ChangeTracker changeTracker, IMongoCollection collection)
            : base(mappingStore, changeTracker, collection)
        { }

        /// <summary>
        /// Finds the one.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public object GetById(Type type, string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Cannot be null or empty.", "id");

            var classMap = this.MappingStore.GetClassMapFor(type);
            if (!classMap.HasId)
                throw new InvalidOperationException("Only entities with identifiers are persistable.");

            //var mappingContext = new MappingContext(Document, type);
            //mappingContext.Document[classMap.IdMap.Key] = classMap.IdMap.ValueType.ConvertToDocumentValue(id, mappingContext);

            throw new NotImplementedException();
            //return this.Find(classMap, mappingContext.Document, 1, 0, new Document(), new Document()).Single();
        }
    }
}