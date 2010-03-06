using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Mapper.Mapping.Visitors;

namespace MongoDB.Mapper.Mapping.Visitors
{
    public class DocumentToEntityDBRefMapper : DocumentToEntityMapper
    {
        private IMongoSessionImplementor mongoSession;

        public DocumentToEntityDBRefMapper(IMongoSessionImplementor mongoSession)
            : base(mongoSession.MappingStore)
        {
            if (mongoSession == null)
                throw new ArgumentNullException("mongoSession");

            this.mongoSession = mongoSession;
        }

        public override void Visit(ManyToOneValueType manyToOneValueType)
        {
            var dbRef = this.value as DBRef;
            if (dbRef == null)
            {
                this.value = null;
                return;
            }

            var referenceClassMap = this.mongoSession.MappingStore.GetClassMapFor(manyToOneValueType.ReferenceType);
            var id = referenceClassMap.IdMap.ValueConverter.FromDocument(dbRef.Id);

            if (this.mongoSession.SessionCache.TryToFind(referenceClassMap.CollectionName, id, out this.value))
                return;

            if (!manyToOneValueType.IsLazy)
                this.value = this.mongoSession.GetById(manyToOneValueType.ReferenceType, id);
            else
                this.value = this.mongoSession.ProxyGenerator.GetProxy(referenceClassMap.Type, id, this.mongoSession);
        }
    }
}