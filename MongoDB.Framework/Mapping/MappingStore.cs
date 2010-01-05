using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class MappingStore : IMappingStore
    {
        private Dictionary<Type, ClassMap> classMaps;

        /// <summary>
        /// Gets the root class maps.
        /// </summary>
        /// <value>The root class maps.</value>
        public IEnumerable<RootClassMap> RootClassMaps
        {
            get { return this.classMaps.Values.OfType<RootClassMap>(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IMappingStore"/> class.
        /// </summary>
        public MappingStore()
            : this(null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IMappingStore"/> class.
        /// </summary>
        /// <param name="rootClassMaps">The root class maps.</param>
        public MappingStore(IEnumerable<RootClassMap> rootClassMaps)
        {
            if (rootClassMaps == null)
                throw new ArgumentNullException("rootClassMaps");

            this.classMaps = new Dictionary<Type, ClassMap>();
            foreach (var rootClassMap in rootClassMaps)
                this.AddRootClassMap(rootClassMap);
        }

        /// <summary>
        /// Adds the root class map.
        /// </summary>
        /// <param name="rootClassMap">The root class map.</param>
        public void AddRootClassMap(RootClassMap rootClassMap)
        {
            if (rootClassMap == null)
                throw new ArgumentNullException("rootClassMap");

            this.classMaps.Add(rootClassMap.Type, rootClassMap);
            foreach (var subClassMap in rootClassMap.SubClassMaps)
                this.classMaps.Add(subClassMap.Type, subClassMap);
        }

        /// <summary>
        /// Gets the class map for.
        /// </summary>
        /// <param name="type">ValueType of the entity.</param>
        /// <returns></returns>
        public virtual ClassMap GetClassMapFor(Type type)
        {
            ClassMap classMap = null;
            //TODO: add hook for runtime creation of map...
            if(!this.classMaps.TryGetValue(type, out classMap))
                throw new UnmappedTypeException(string.Format("The type {0} is unmapped.", type));

            return classMap;
        }
    }
}