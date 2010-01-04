using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Configuration.Mapping.Visitors
{
    public class IdToEntityMapper : DefaultMapVisitor
    {
        private Document document;
        private object entity;
        private IMongoContextImplementor mongoContext;

        public IdToEntityMapper(IMongoContextImplementor mongoContext)
        {
            if (mongoContext == null)
                throw new ArgumentNullException("mongoContext");

            this.mongoContext = mongoContext;
        }

        public void ApplyId(ClassMap classMap, Document document, object entity)
        {
            if (classMap == null)
	            throw new ArgumentNullException("classMap");
            if (document == null)
	            throw new ArgumentNullException("document");
            if (entity == null)
                throw new ArgumentNullException("entity");

            this.document = document;
            this.entity = entity;
            classMap.Accept(this);
        }

        public override void ProcessId(IdMap idMap)
        {
            var value = this.document[idMap.Key];
            value = idMap.ValueType.ConvertFromDocumentValue(value, this.mongoContext);
            idMap.MemberSetter(this.entity, value);
        }


    }
}