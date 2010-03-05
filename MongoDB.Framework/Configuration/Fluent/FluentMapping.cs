using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping.Auto;
using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Configuration.Fluent.Mapping;
using System.Reflection;

namespace MongoDB.Framework.Configuration.Fluent
{
    public class FluentMapping : IMappingStoreBuilder
    {
        private IAutoMapper autoMapper;
        private FluentMapModelRegistry registry;

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

        /// <summary>
        /// Adds the maps from assembly containing.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public FluentMapping AddMapsFromAssemblyContaining<T>()
        {
            this.registry.AddMapsFromAssemblyContaining<T>();
            return this;
        }

        /// <summary>
        /// Adds the maps from assembly.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public FluentMapping AddMapsFromAssembly(Assembly assembly)
        {
            this.registry.AddMapsFromAssembly(assembly);
            return this;
        }

        IMappingStore IMappingStoreBuilder.BuildMappingStore()
        {
            return new AutoMappingStore(this.autoMapper, this.registry.BuildMappingStore());
        }
    }
}