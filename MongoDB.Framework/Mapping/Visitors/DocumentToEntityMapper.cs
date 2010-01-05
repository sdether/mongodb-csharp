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
            value = memberMap.ValueType.ConvertFromDocumentValue(value, this.mongoSession);
            memberMap.MemberSetter(this.entity, value);
            this.document.Remove(memberMap.Key);
        }

        public override void ProcessManyToOne(ManyToOneMap manyToOneMap)
        {
            var value = document[manyToOneMap.Key] as DBRef;
            this.document.Remove(manyToOneMap.Key);
            if (value == null)
                return;

            var referenceClassMap = this.mongoSession.MappingStore.GetClassMapFor(manyToOneMap.ReferenceType);
            var id = referenceClassMap.IdMap.ValueType.ConvertFromDocumentValue(value.Id, this.mongoSession);

            object referencedEntity = null;
            if (!manyToOneMap.IsLazy)
            {
                referencedEntity = this.mongoSession.GetById(manyToOneMap.ReferenceType, id);
            }
            else
            {
                referencedEntity = this.mongoSession.ProxyGenerator.GetProxy(referenceClassMap.Type, id, this.mongoSession);
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