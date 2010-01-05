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
        private IMongoContextImplementor mongoContext;

        public DocumentToEntityMapper(IMongoContextImplementor mongoContext)
        {
            if (mongoContext == null)
                throw new ArgumentNullException("mongoContext");

            this.mongoContext = mongoContext;
        }

        public object CreateEntity(ClassMap classMap, Document document)
        {
            if (classMap == null)
                throw new ArgumentNullException("classMap");
            if (document == null)
                throw new ArgumentNullException("document");

            this.document = document;
            this.entity = Activator.CreateInstance(classMap.Type);

            classMap.Accept(this);

            return entity;
        }

        public override void ProcessClass(ClassMap classMap)
        {
            if (classMap.IsPolymorphic)
                document.Remove(classMap.DiscriminatorKey);
        }

        public override void ProcessMember(MemberMap memberMap)
        {
            var value = document[memberMap.Key];
            value = memberMap.ValueType.ConvertFromDocumentValue(value, this.mongoContext);
            memberMap.MemberSetter(this.entity, value);
            this.document.Remove(memberMap.Key);
        }

        public override void ProcessManyToOne(ManyToOneMap manyToOneMap)
        {
            var value = document[manyToOneMap.Key] as DBRef;
            this.document.Remove(manyToOneMap.Key);
            if (value == null)
                return;

            var referenceClassMap = this.mongoContext.MappingStore.GetClassMapFor(manyToOneMap.ReferenceType);
            var id = referenceClassMap.IdMap.ValueType.ConvertFromDocumentValue(value.Id, this.mongoContext);

            object referencedEntity = null;
            if (!manyToOneMap.IsLazy)
            {
                referencedEntity = this.mongoContext.GetById(manyToOneMap.ReferenceType, id);
            }
            else
            {
                referencedEntity = this.mongoContext.ProxyGenerator.GetProxy(referenceClassMap.Type, id, this.mongoContext);
            }


            manyToOneMap.MemberSetter(this.entity, referencedEntity);
        }

        public override void ProcessExtendedProperties(ExtendedPropertiesMap extendedPropertiesMap)
        {
            var dictionary = this.document.ToDictionary();
            extendedPropertiesMap.MemberSetter(this.entity, dictionary);
        }
    }
}