using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Mapping.Visitors;

namespace MongoDB.Framework.Mapping.Visitors
{
    public class DocumentToEntityMapper : DefaultMapVisitor
    {
        private Document document;
        private object entity;

        private object value;
        private IMongoSessionImplementor mongoSession;

        public DocumentToEntityMapper(IMongoSessionImplementor mongoSession)
        {
            if (mongoSession == null)
                throw new ArgumentNullException("mongoSession");

            this.mongoSession = mongoSession;
        }

        public object CreateEntity(ClassMap classMap, Document document)
        {
            if (classMap == null)
                throw new ArgumentNullException("classMap");
            if (document == null)
                throw new ArgumentNullException("document");

            this.document = document;

            classMap.Accept(this);

            return this.entity;
        }

        public override void Visit(ClassMap classMap)
        {
            this.entity = Activator.CreateInstance(classMap.Type);

            base.Visit(classMap);
        }

        public override void Visit(IdMap idMap)
        {
            var value = this.document[idMap.Key];
            value = idMap.ValueConverter.FromDocument(value);
            idMap.MemberSetter(this.entity, value);
            this.document.Remove(idMap.Key);
        }

        public override void Visit(string discriminatorKey, object discriminator)
        {
            base.Visit(discriminatorKey, discriminator);

            document.Remove(discriminatorKey);
        }

        public override void Visit(MemberMap memberMap)
        {
            var oldValue = this.value;
            this.value = this.document[memberMap.Key];
            base.Visit(memberMap);
            memberMap.MemberSetter(this.entity, this.value);
            this.value = oldValue;
            this.document.Remove(memberMap.Key);
        }

        public override void Visit(ExtendedPropertiesMap extendedPropertiesMap)
        {
            var dictionary = this.document.ToDictionary();
            extendedPropertiesMap.MemberSetter(this.entity, dictionary);
        }

        public override void Visit(SimpleValueType simpleValueType)
        {
            this.value = simpleValueType.ValueConverter.FromDocument(this.value);
        }

        public override void Visit(NestedClassValueType nestedClassValueType)
        {
            var doc = this.value as Document;
            if (doc == null)
            {
                this.value = null;
                return;
            }

            var oldEntity = this.entity;
            var oldDocument = this.document;

            this.document = doc;
            ClassMap concreteClassMap = nestedClassValueType.NestedClassMap;
            if (nestedClassValueType.NestedClassMap.IsPolymorphic)
            {
                object discriminator = this.document[nestedClassValueType.NestedClassMap.DiscriminatorKey];
                concreteClassMap = nestedClassValueType.NestedClassMap.GetClassMapByDiscriminator(discriminator);
            }

            concreteClassMap.Accept(this);
            
            this.value = this.entity;
            this.entity = oldEntity;
            this.document = oldDocument;
        }

        public override void Visit(CollectionValueType collectionValueType)
        {
            var collectionElements = collectionValueType.CollectionType.BreakDocumentValueIntoElements(this.value);
            var convertedElements = new List<CollectionElement>();
            foreach (var collectionElement in collectionElements)
            {
                this.value = collectionElement.Element;
                collectionValueType.ElementValueType.Accept(this);
                convertedElements.Add(new CollectionElement() { Element = this.value, CustomData = collectionElement.CustomData });
            }

            this.value = collectionValueType.CollectionType.CreateCollection(collectionValueType.ElementValueType.Type, convertedElements);
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