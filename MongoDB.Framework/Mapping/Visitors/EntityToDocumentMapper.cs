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
        private object value;
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

        public override void Visit(ClassMap classMap)
        {
            if (classMap.IsPolymorphic)
                document[classMap.DiscriminatorKey] = classMap.Discriminator;

            base.Visit(classMap);
        }

        public override void Visit(IdMap idMap)
        {
            var value = idMap.MemberGetter(this.entity);
            value = idMap.ValueConverter.ToDocument(value);
            this.document[idMap.Key] = value;
        }

        public override void Visit(MemberMap memberMap)
        {
            var oldValue = this.value;
            this.value = memberMap.MemberGetter(this.entity);
            base.Visit(memberMap);
            if(this.value != MongoDBNull.Value || memberMap.PersistNull)
                this.document[memberMap.Key] = this.value;
            this.value = oldValue;
        }

        public override void Visit(ExtendedPropertiesMap extendedPropertiesMap)
        {
            var dictionary = (IDictionary<string, object>)extendedPropertiesMap.MemberGetter(this.entity);

            dictionary.ToDocument()
                .CopyTo(this.document);
        }

        public override void Visit(SimpleValueType simpleValueType)
        {
            this.value = simpleValueType.ValueConverter.ToDocument(this.value);
        }

        public override void Visit(NestedClassValueType nestedClassValueType)
        {
            var oldEntity = this.entity;
            var oldDocument = this.document;
            
            this.entity = this.value;
            this.document = new Document();

            nestedClassValueType.NestedClassMap.Accept(this);

            this.value = this.document;
            this.entity = oldEntity;
            this.document = oldDocument;
        }

        public override void Visit(CollectionValueType collectionValueType)
        {
            var collectionElements = collectionValueType.CollectionType.BreakCollectionIntoElements(collectionValueType.ElementValueType.Type, this.value);
            foreach (var collectionElement in collectionElements)
            {
                this.value = collectionElement.Element;
                collectionValueType.ElementValueType.Accept(this);
                collectionElement.Element = this.value;
            }

            this.value = collectionValueType.CollectionType.CreateDocumentValueFromElements(collectionElements);
        }

        public override void Visit(ManyToOneValueType manyToOneValueType)
        {
            var referenceClassMap = this.mongoSession.MappingStore.GetClassMapFor(manyToOneValueType.ReferenceType);
            if (this.value != null)
                this.value = new DBRef(referenceClassMap.CollectionName, referenceClassMap.GetId(this.value));
            else
                this.value = MongoDBNull.Value;
        }
    }
}