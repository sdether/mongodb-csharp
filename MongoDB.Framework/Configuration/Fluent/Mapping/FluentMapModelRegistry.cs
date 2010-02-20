using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Reflection;
using MongoDB.Framework.Configuration.Fluent.Mapping.Auto;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    public class FluentMapModelRegistry : MapModelRegistry
    {
        public FluentMapModelRegistry WithAssemblyContainingType<T>(Action<FluentAssembly> config)
        {
            return this.WithAssembly(typeof(T).Assembly, config);
        }

        /// <summary>
        /// Withes the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public FluentMapModelRegistry WithAssembly(Assembly assembly, Action<FluentAssembly> config)
        {
            config(new FluentAssembly(this, assembly));
            return this;
        }

        /// <summary>
        /// Autoes the map as root class.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public FluentMapModelRegistry AutoMapAsRootClass<T>(Action<FluentAutoMap> config)
        {
            var model = new RootClassMapModel(typeof(T));
            config(new FluentAutoMap(model.AutoMap));
            this.AddRootClassMapModel(model);
            return this;
        }

        /// <summary>
        /// Adds the map.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rootClassMap">The root class map.</param>
        /// <returns></returns>
        public FluentMapModelRegistry AddMap<T>(FluentRootClass<T> rootClassMap)
        {
            this.AddRootClassMapModel(rootClassMap.Model);
            return this;
        }

        /// <summary>
        /// Adds the map.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nestedClassMap">The nested class map.</param>
        /// <returns></returns>
        public FluentMapModelRegistry AddMap<T>(FluentNestedClass<T> nestedClassMap)
        {
            this.AddNestedClassMapModel(nestedClassMap.Model);
            return this;
        }

        /// <summary>
        /// Adds the map.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subClassMap">The sub class map.</param>
        /// <returns></returns>
        public FluentMapModelRegistry AddMap<T>(FluentSubClass<T> subClassMap)
        {
            this.AddSubClassMapModel(subClassMap.Model);
            return this;
        }

    }
}