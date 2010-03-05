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
        private IAutoMapper autoMapper;
        private FluentMapModelRegistry mapModelRegistry;

        public FluentConfiguration WithAutoMappingProfile(Action<FluentAutoMappingProfile> config)
        {
            return this.WithAutoMappingProfileWhen(t => true, config);
        }

        public FluentConfiguration WithAutoMappingProfileWhen(Func<Type, bool> filter, Action<FluentAutoMappingProfile> config)
        {
            var profile = new AutoMappingProfile();
            config(new FluentAutoMappingProfile(profile));
            return this.WithAutoMappingProfileWhen(filter, profile);
        }

        public FluentConfiguration WithAutoMappingProfile(AutoMappingProfile profile)
        {
            return this.WithAutoMappingProfileWhen(t => true, profile);
        }

        public FluentConfiguration WithAutoMappingProfileWhen(Func<Type, bool> filter, AutoMappingProfile profile)
        {
            return this.WithAutoMapper(new AutoMapper(profile, filter));
        }

        public FluentConfiguration WithAutoMapper(IAutoMapper autoMapper)
        {
            if (this.autoMapper == null)
                this.autoMapper = autoMapper;
            else if (this.autoMapper is AggregateAutoMapper)
                ((AggregateAutoMapper)this.autoMapper).AddAutoMapper(autoMapper);
            else
            {
                AggregateAutoMapper aam = new AggregateAutoMapper();
                aam.AddAutoMapper(this.autoMapper);
                aam.AddAutoMapper(autoMapper);
                this.autoMapper = aam;
            }

            return this;
        }

        public FluentConfiguration Database(string databaseName)
        {
            this.databaseName = databaseName;
            return this;
        }

        public IMongoSessionFactory BuildSessionFactory()
        {
            return new MongoSessionFactory(
                this.databaseName,
                new AutoMappingStore(this.autoMapper ?? new AutoMapper()),
                new DefaultMongoFactory(),
                new CastleProxyGenerator());
        }
    }
}