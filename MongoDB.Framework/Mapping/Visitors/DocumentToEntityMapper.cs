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
        protected Document document;
        protected object parentEntity;
        protected object entity;

        protected object value;

        public DocumentToEntityMapper()
        {
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

        public override void Visit(ValueTypeMemberMap memberMap)
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

        public override void Visit(ParentMemberMap parentMemberMap)
        {
            parentMemberMap.MemberSetter(this.entity, this.parentEntity);
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

            var oldParentEntity = this.parentEntity;
            this.parentEntity = this.entity;
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
            this.entity = this.parentEntity;
            this.parentEntity = oldParentEntity;
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
            throw new InvalidOperationException("DocumentToEntityMapper cannot be used when dbref's exist.");
        }
    }
}