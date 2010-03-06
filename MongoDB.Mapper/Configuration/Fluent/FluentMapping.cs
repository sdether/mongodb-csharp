using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Mapper.Mapping.Auto;
using MongoDB.Mapper.Configuration.Mapping;
using MongoDB.Mapper.Mapping;
using MongoDB.Mapper.Configuration.Fluent.Mapping;
using System.Reflection;

namespace MongoDB.Mapper.Configuration.Fluent
{
    public class FluentMapping : IMappingStoreBuilder
    {
        private IAutoMapper autoMapper;
        private bool noAutoMapping;
        private FluentMapModelRegistry registry;
        private List<Type> typesToAutoMap;

        public FluentMapModelRegistry Registry
        {
            get 
            {
                if (this.registry == null)
                    this.registry = new FluentMapModelRegistry();
                return this.registry; 
            }
            set { this.registry = value; }
        }

        public FluentMapping AddAutoMapper(IAutoMapper autoMapper)
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

        public FluentMapping CreateProfile(Action<FluentAutoMappingProfile> config)
        {
            return this.CreateFilteredProfile(t => true, config);
        }

        public FluentMapping CreateFilteredProfile(Func<Type, bool> filter, Action<FluentAutoMappingProfile> config)
        {
            var profile = new AutoMappingProfile();
            config(new FluentAutoMappingProfile(profile));
            return this.UseFilteredProfile(filter, profile);
        }

        public FluentMapping UseProfile(AutoMappingProfile profile)
        {
            return this.UseFilteredProfile(t => true, profile);
        }

        public FluentMapping UseFilteredProfile(Func<Type, bool> filter, AutoMappingProfile profile)
        {
            return this.AddAutoMapper(new AutoMapper(profile, filter));
        }

        public FluentMapping EagerlyAutoMap<T>()
        {
            return this.EagerlyAutoMapTypes(new Type[1] { typeof(T) });
        }

        public FluentMapping EagerlyAutoMapTypes(IEnumerable<Type> types)
        {
            if (this.typesToAutoMap == null)
                this.typesToAutoMap = new List<Type>();

            this.typesToAutoMap.AddRange(types);
            return this;
        }

        public FluentMapping NoAutoMapping()
        {
            this.noAutoMapping = true;
            return this;
        }

        IMappingStore IMappingStoreBuilder.BuildMappingStore()
        {
            if (this.autoMapper == null && this.registry == null)
            {
                if (this.noAutoMapping)
                    throw new InvalidOperationException("Either AutoMapping should be enabled or maps should be added through the registry.");

                return this.AutoMapEagerTypes(new AutoMappingStore());
            }
            else if (this.autoMapper == null && this.noAutoMapping)
                return this.registry.BuildMappingStore();
            else if (this.autoMapper == null)
                return this.AutoMapEagerTypes(new AutoMappingStore(new AutoMapper(), this.registry.BuildMappingStore()));
            else if (this.registry == null)
                return this.AutoMapEagerTypes(new AutoMappingStore(this.autoMapper));

            return this.AutoMapEagerTypes(new AutoMappingStore(this.autoMapper, this.registry.BuildMappingStore()));
        }

        private AutoMappingStore AutoMapEagerTypes(AutoMappingStore store)
        {
            if(typesToAutoMap == null)
                return store;

            foreach (var type in this.typesToAutoMap)
                store.GetClassMapFor(type);

            return store;
        }
    }
}