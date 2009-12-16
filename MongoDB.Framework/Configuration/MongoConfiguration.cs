using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration
{
    public class MongoConfiguration
    {
        #region Private Fields

        private Dictionary<Type, RootEntityMap> rootEntityMaps;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        /// <value>The name of the database.</value>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets the root entity maps.
        /// </summary>
        /// <value>The root entity maps.</value>
        public IEnumerable<RootEntityMap> RootEntityMaps
        {
            get { return this.rootEntityMaps.Values.Distinct(); }
        }
 
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoConfiguration"/> class.
        /// </summary>
        public MongoConfiguration()
        {
            this.rootEntityMaps = new Dictionary<Type, RootEntityMap>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the root entity map.
        /// </summary>
        /// <param name="rootEntityMap">The root entity map.</param>
        public void AddRootEntityMap(RootEntityMap rootEntityMap)
        {
            this.rootEntityMaps.Add(rootEntityMap.Type, rootEntityMap);
            foreach(var map in rootEntityMap.DiscriminatedEntityMaps)
                this.rootEntityMaps.Add(map.Type, rootEntityMap);
        }

        /// <summary>
        /// Gets the root entity map for the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public RootEntityMap GetRootEntityMapFor<T>()
        {
            return this.GetRootEntityMapFor(typeof(T));
        }

        /// <summary>
        /// Gets the root entity map for the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public RootEntityMap GetRootEntityMapFor(Type type)
        {
            RootEntityMap rootEntityMap = null;
            if (!this.rootEntityMaps.TryGetValue(type, out rootEntityMap))
                throw new UnmappedTypeException(string.Format("No root entity map found for type {0}", type));

            return rootEntityMap;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Called when a root entity map could not be found.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="rootEntityMap">The root entity map.</param>
        /// <returns></returns>
        protected virtual bool OnMissingEntityMap(Type type, out RootEntityMap rootEntityMap)
        {
            rootEntityMap = null;
            return false;
        }

        #endregion
    }
}