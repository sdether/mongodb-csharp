using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.IdGenerators
{
    public class AssignedGenerator : IIdGenerator
    {
        public object Generate(object entity, IMongoContext mongoContext)
        {
            var classMap = mongoContext.Configuration.IMappingStore.GetClassMapFor(entity.GetType());
            object id = classMap.IdMap.MemberGetter(entity);
            if (Object.Equals(id, classMap.IdMap.UnsavedValue))
                throw new IdGenerationException(string.Format("Ids for {0} must be manually assigned before saving.", classMap.Type));

            return id;
        }
    }
}