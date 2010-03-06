using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Mapper.Mapping.Visitors
{
    public class ValueToDocumentValueMapper : DefaultMapVisitor
    {
        private Document document;
        private object value;
        private IMongoSessionImplementor mongoSession;

        public ValueToDocumentValueMapper(IMongoSessionImplementor mongoSession)
        {
            if (mongoSession == null)
                throw new ArgumentNullException("mongoSession");

            this.mongoSession = mongoSession;
        }

        public object CreateDocumentValue(PersistentMemberMap memberMapBase, object value)
        {
            if (memberMapBase == null)
	            throw new ArgumentNullException("memberMapBase");

            this.value = value;
            memberMapBase.Accept(this);
            return this.value;
        }

        public override void Visit(IdMap idMap)
        {
            this.value = idMap.ValueConverter.ToDocument(value);
        }

        public override void Visit(SimpleValueType simpleValueType)
        {
            this.value = simpleValueType.ValueConverter.ToDocument(this.value);
        }

        public override void Visit(NestedClassValueType nestedClassValueType)
        {
            this.value = this.mongoSession.MapToDocument(this.value);
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
            var referenceClassMap = this.mongoSession.MappingStore.GetClassMapFor(manyToOneValueType.ReferenceType);
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