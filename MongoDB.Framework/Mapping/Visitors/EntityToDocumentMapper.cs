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
        private IMappingStore mappingStore;

        public EntityToDocumentMapper(IMappingStore mappingStore)
        {
            if (mappingStore == null)
                throw new ArgumentNullException("mappingStore");

            this.mappingStore = mappingStore;
        }

        public Document CreateDocument(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var classMap = this.mappingStore.GetClassMapFor(entity.GetType());
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

        public override void Visit(IdMap idMap)
        {
            var value = idMap.MemberGetter(this.entity);
            value = idMap.ValueConverter.ToDocument(value);
            this.document[idMap.Key] = value;
        }

        public override void Visit(string discriminatorKey, object discriminator)
        {
            base.Visit(discriminatorKey, discriminator);

            document[discriminatorKey] = discriminator;
        }

        public override void Visit(ValueTypeMemberMap memberMap)
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
            if (this.value == null)
                this.value = MongoDBNull.Value;

            var oldEntity = this.entity;
            var oldDocument = this.document;
            
            this.entity = this.value;
            this.document = new Document();

            var concreteClassMap = nestedClassValueType.NestedClassMap.GetClassMapFor(this.entity.GetType());
            concreteClassMap.Accept(this);

            this.value = this.document;
            this.entity = oldEntity;
            this.document = oldDocument;
        }

        public override void Visit(CollectionValueType collectionValueType)
        {
            var collectionElements = collectionValueType.CollectionType.BreakCollectionIntoElements(collectionValueType.ElementValueType.Type, this.value);
            var convertedElements = new List<CollectionElement>();
            foreach (var collectionElement in collectionElements)
            {
                this.value = collectionElement.Element;
                collectionValueType.ElementValueType.Accept(this);
                convertedElements.Add(new CollectionElement() { Element = this.value, CustomData = collectionElement.CustomData });
            }

            this.value = collectionValueType.CollectionType.CreateDocumentValueFromElements(convertedElements);
        }

        public override void Visit(ManyToOneValueType manyToOneValueType)
        {
            var referenceClassMap = this.mappingStore.GetClassMapFor(manyToOneValueType.ReferenceType);
            if (this.value != null)
            {
                var id = referenceClassMap.IdMap.ValueConverter.ToDocument(referenceClassMap.GetId(this.value));
                this.value = new DBRef(referenceClassMap.CollectionName, id);
            }
            else
                this.value = MongoDBNull.Value;
        }
    }
}