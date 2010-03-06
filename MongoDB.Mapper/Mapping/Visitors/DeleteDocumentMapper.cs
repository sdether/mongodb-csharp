using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Mapper.Mapping.Visitors
{
    public class DeleteDocumentMapper : DefaultMapVisitor
    {
        private Document document;
        private object entity;
        private IMongoSessionImplementor mongoSession;

        public DeleteDocumentMapper(IMongoSessionImplementor mongoSession)
        {
            if (mongoSession == null)
                throw new ArgumentNullException("mongoSession");

            this.mongoSession = mongoSession;
        }

        public Document CreateDocument(ClassMapBase classMap, object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            this.entity = entity;

            this.document = new Document();
            classMap.Accept(this);

            return this.document;
        }

        public override void Visit(IdMap idMap)
        {
            var value = idMap.MemberGetter(this.entity);
            value = idMap.ValueConverter.ToDocument(value);
            this.document[idMap.Key] = value;
        }


    }
}