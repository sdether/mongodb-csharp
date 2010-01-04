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
    public class GetByIdAction : FindActionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdAction"/> class.
        /// </summary>
        /// <param name="mongoContext">The mongoContext.</param>
        /// <param name="mongoContextCache">The mongo context cache.</param>
        /// <param name="changeTracker">The change tracker.</param>
        public GetByIdAction(IMongoContextImplementor mongoContext, IMongoContextCache mongoContextCache, IChangeTracker changeTracker)
            : base(mongoContext, mongoContextCache, changeTracker)
        { }

        /// <summary>
        /// Finds the one.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public object GetById(Type type, object id)
        {
            var classMap = this.MongoContext.MappingStore.GetClassMapFor(type);
            var idValue = classMap.IdMap.ValueType.ConvertToDocumentValue(id, null);
            var conditions = new Document().Append("_id", idValue);
            var entity = this.Find(classMap, conditions, 1, 0, null, null).SingleOrDefault();
            if(entity == null)
                throw new EntityNotFoundException(string.Format("A {0} was not found with the id {1}.", type, id));
            return entity;
        }
    }
}