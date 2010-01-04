using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping
{
    public class MappingStore : IMappingStore
    {
        private Dictionary<Type, ClassMap> classMaps;
        private List<IMapProvider> mapProviders;

        /// <summary>
        /// Initializes a new instance of the <see cref="IMappingStore"/> class.
        /// </summary>
        public MappingStore()
            : this(null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IMappingStore"/> class.
        /// </summary>
        /// <param name="mapProviders">The map providers.</param>
        public MappingStore(params IMapProvider[] mapProviders)
            : this((IEnumerable<IMapProvider>)mapProviders)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IMappingStore"/> class.
        /// </summary>
        /// <param name="mapProviders">The map providers.</param>
        public MappingStore(IEnumerable<IMapProvider> mapProviders)
        {
            this.classMaps = new Dictionary<Type, ClassMap>();
            this.mapProviders = new List<IMapProvider>(mapProviders ?? Enumerable.Empty<IMapProvider>());
        }

        /// <summary>
        /// Adds the map provider.
        /// </summary>
        /// <param name="mapProvider">The map provider.</param>
        public void AddMapProvider(IMapProvider mapProvider)
        {
            if (mapProviders == null)
                throw new ArgumentNullException("mapProviders");

            this.mapProviders.Add(mapProvider);
        }

        /// <summary>
        /// Gets the class map for.
        /// </summary>
        /// <param name="type">ValueType of the entity.</param>
        /// <returns></returns>
        public virtual ClassMap GetClassMapFor(Type type)
        {
            ClassMap classMap = null;
            if(!this.TryGetClassMap(type, out classMap))
                throw new UnmappedTypeException(string.Format("The type {0} is unmapped.", type));

            return classMap;
        }

        /// <summary>
        /// Tries the get class map.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="classMap">The class map.</param>
        /// <returns></returns>
        private bool TryGetClassMap(Type type, out ClassMap classMap)
        {
            if (this.classMaps.TryGetValue(type, out classMap))
                return true;

            foreach (var mapProvider in this.mapProviders)
            {
                var rootClassMap = mapProvider.GetRootClassMapFor(type);
                if (rootClassMap != null)
                {
                    this.classMaps.Add(rootClassMap.Type, rootClassMap);
                    foreach (var subClassMap in rootClassMap.SubClassMaps)
                        this.classMaps.Add(subClassMap.Type, subClassMap);
                }
            }

            return this.classMaps.TryGetValue(type, out classMap);
        }
    }
}