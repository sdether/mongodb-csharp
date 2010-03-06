using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Mapper.Mapping;
using MongoDB.Mapper.Mapping.Visitors;

namespace MongoDB.Mapper
{
    public class MongoMapper : IMongoMapper
    {
        private IMappingStore mappingStore;

        public MongoMapper(IMappingStore mappingStore)
        {
            if (mappingStore == null)
                throw new ArgumentNullException("mappingStore");

            this.mappingStore = mappingStore;
        }

        public Document MapToDocument(object entity)
        {
            return new EntityToDocumentMapper(this.mappingStore).CreateDocument(entity);
        }

        public object MapToEntity(Type entityType, Document document)
        {
            var classMap = this.mappingStore.GetClassMapFor(entityType);
            if (classMap.IsPolymorphic)
            {
                var discriminator = document[classMap.DiscriminatorKey];
                classMap = classMap.GetClassMapByDiscriminator(discriminator);
            }
            return new DocumentToEntityMapper(this.mappingStore).CreateEntity(classMap, document);
        }
    }
}