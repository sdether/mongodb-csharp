using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Mapper.Configuration.Mapping;
using MongoDB.Mapper.Configuration.Fluent.Mapping;
using MongoDB.Mapper.Mapping.Auto;
using MongoDB.Mapper.Mapping;
using MongoDB.Mapper.Proxy.Castle;
using MongoDB.Mapper.Proxy;

namespace MongoDB.Mapper.Configuration.Fluent
{
    public class FluentConfiguration
    {
        private string databaseName;
        private IMappingStore mappingStore;
        private IMongoFactory mongoFactory;
        private IProxyGenerator proxyGenerator;

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

        public FluentConfiguration MongoFactory(IMongoFactory mongoFactory)
        {
            this.mongoFactory = mongoFactory;
            return this;
        }

        public FluentConfiguration ProxyGenerator(IProxyGenerator proxyGenerator)
        {
            this.proxyGenerator = proxyGenerator;
            return this;
        }

        public IMongoSessionFactory BuildSessionFactory()
        {
            return new MongoSessionFactory(
                this.databaseName,
                this.mappingStore ?? new AutoMappingStore(),
                this.mongoFactory ?? new DefaultMongoFactory(),
                this.proxyGenerator ?? new CastleProxyGenerator());
        }
    }
}