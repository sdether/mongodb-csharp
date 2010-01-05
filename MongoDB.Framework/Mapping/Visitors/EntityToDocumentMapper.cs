using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;

namespace MongoDB.Framework.Mapping.Visitors
{
    public class EntityToDocumentMapper : DefaultMapVisitor
    {
        private Document document;
        private object entity;
        private IMongoSessionImplementor mongoSession;

        public EntityToDocumentMapper(IMongoSessionImplementor mongoSession)
        {
            if (mongoSession == null)
                throw new ArgumentNullException("mongoSession");

            this.mongoSession = mongoSession;
        }

        public Document CreateDocument(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var classMap = this.mongoSession.MappingStore.GetClassMapFor(entity.GetType());
            return this.CreateDocument(classMap, entity);
        }

        public Document CreateDocument(ClassMap classMap, object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            this.document = new Document();
            this.entity = entity;

            classMap.Accept(this);
            return this.document;
        }

        public override void ProcessClass(ClassMap classMap)
        {
            if (classMap.IsPolymorphic)
                document[classMap.DiscriminatorKey] = classMap.Discriminator;
        }

        public override void ProcessManyToOne(ManyToOneMap manyToOneMap)
        {
            object value = MongoDBNull.Value;
            var referenceEntity = manyToOneMap.MemberGetter(this.entity);
            if (referenceEntity != null)
            {
                var referenceClassMap = this.mongoSession.MappingStore.GetClassMapFor(manyToOneMap.ReferenceType);
                var id = referenceClassMap.IdMap.ValueType.ConvertToDocumentValue(referenceClassMap.GetId(referenceEntity), this.mongoSession);
                value = new DBRef(referenceClassMap.CollectionName, id);
            }

            if(referenceEntity != null || manyToOneMap.PersistNull)
                this.document[manyToOneMap.Key] = value;
        }

        public override void ProcessMember(MemberMap memberMap)
        {
            var value = memberMap.MemberGetter(this.entity);
            value = memberMap.ValueType.ConvertToDocumentValue(value, this.mongoSession);
            if(value != MongoDBNull.Value || memberMap.PersistNull)
                this.document[memberMap.Key] = value;
        }

        public override void ProcessExtendedProperties(ExtendedPropertiesMap extendedPropertiesMap)
        {
            var dictionary = (IDictionary<string, object>)extendedPropertiesMap.MemberGetter(this.entity);

            dictionary.ToDocument()
                .CopyTo(this.document);
        }
    }
}