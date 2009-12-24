using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public abstract class MappingStore
    {
        private Dictionary<Type, ClassMap> classMaps;

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingStore"/> class.
        /// </summary>
        public MappingStore()
        {
            this.classMaps = new Dictionary<Type, ClassMap>();
        }

        /// <summary>
        /// Adds the collection map.
        /// </summary>
        /// <param name="collectionMap">The collection map.</param>
        public void AddCollectionMap(RootClassMap collectionMap)
        {
            if (collectionMap == null)
                throw new ArgumentNullException("collectionMap");

            this.classMaps.Add(collectionMap.Type, collectionMap);
            foreach (var subClassMap in collectionMap.SubClassMaps)
                this.classMaps.Add(subClassMap.Type, subClassMap);
        }

        /// <summary>
        /// Gets the class map for.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public ClassMap GetClassMapFor<TEntity>()
        {
            return this.GetClassMapFor(typeof(TEntity));
        }

        /// <summary>
        /// Gets the class map for.
        /// </summary>
        /// <param name="type">Type of the entity.</param>
        /// <returns></returns>
        public ClassMap GetClassMapFor(Type type)
        {
            ClassMap classMap = null;
            if(!this.classMaps.TryGetValue(type, out classMap))
            {
                if (!this.TryGetMissingClassMap(type, out classMap))
                    throw new UnmappedTypeException(string.Format("The type {0} is unmapped.", type));
            }

            return classMap;
        }

        /// <summary>
        /// Tries to get the missing class map.
        /// </summary>
        /// <param name="type">Type of the entity.</param>
        /// <param name="classMap">The class map.</param>
        /// <returns></returns>
        protected abstract bool TryGetMissingClassMap(Type type, out ClassMap classMap);
    }
}