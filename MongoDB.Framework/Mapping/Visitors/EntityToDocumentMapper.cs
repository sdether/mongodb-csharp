using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping.Visitors;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping.Visitors
{
    public class EntityToDocumentMapper : TranslationVisitor
    {
        private Document document;
        private object entity;
        private IMongoContext mongoContext;

        public EntityToDocumentMapper(IMongoContext mongoContext)
        {
            if (mongoContext == null)
                throw new ArgumentNullException("mongoContext");

            this.mongoContext = mongoContext;
        }

        public Document CreateDocument(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var classMap = this.mongoContext.MappingStore.GetClassMapFor(entity.GetType());
            return this.CreateDocument(classMap, entity);
        }

        public Document CreateDocument(ClassMap classMap, object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            this.document = new Document();
            this.entity = entity;

            if (classMap.IsPolymorphic)
                document[classMap.DiscriminatorKey] = classMap.Discriminator;

            classMap.Accept(this);
            return this.document;
        }

        public override void ProcessMember(MemberMap memberMap)
        {
            var value = memberMap.MemberGetter(this.entity);
            value = memberMap.ValueType.ConvertToDocumentValue(value, this.mongoContext);
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