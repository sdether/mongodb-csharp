using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Configuration.Fluent.Mapping;
using MongoDB.Framework.Mapping.Auto;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Proxy.Castle;

namespace MongoDB.Framework.Configuration.Fluent
{
    public class FluentConfiguration
    {
        private string databaseName;
        private IMappingStore mappingStore;

        public FluentConfiguration Database(string databaseName)
        {
            this.databaseName = databaseName;
            return this;
        }

        public FluentConfiguration Mappings(Action<FluentMapping> config)
        {
            var fluentMapping = new FluentMapping();
            config(fluentMapping);
            this.mappingStore = ((IMappingStoreBuilder)fluentMapping).BuildMappingStore();
            return this;
        }

        public IMongoSessionFactory BuildSessionFactory()
        {
            return new MongoSessionFactory(
                this.databaseName,
                this.mappingStore ?? new AutoMappingStore(),
                new DefaultMongoFactory(),
                new CastleProxyGenerator());
        }
    }
}