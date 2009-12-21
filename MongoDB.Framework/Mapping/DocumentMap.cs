using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public abstract class DocumentMap : Map
    {
        #region Private Fields

        private readonly Dictionary<string, ValueMap> valueMaps;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the discriminator.
        /// </summary>
        /// <value>The discriminator.</value>
        public object Discriminator { get; set; }

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <value>The type of the entity.</value>
        public Type EntityType { get; private set; }

        /// <summary>
        /// Gets the value maps.
        /// </summary>
        /// <value>The value maps.</value>
        public virtual IEnumerable<ValueMap> ValueMaps
        {
            get
            {
                foreach (var valueMap in this.valueMaps.Values)
                    yield return valueMap;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentMap"/> class.
        /// </summary>
        /// <param name="metaDataStore">The meta data store.</param>
        /// <param name="entityType">Type of the entity.</param>
        public DocumentMap(MappingStore metaDataStore, Type entityType)
            : base(metaDataStore)
        {
            if (entityType == null)
                throw new ArgumentNullException("entityType");

            this.EntityType = entityType;
            this.valueMaps = new Dictionary<string, ValueMap>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the value map.
        /// </summary>
        /// <param name="valueMap">The value map.</param>
        public void AddValueMap(ValueMap valueMap)
        {
            if (valueMap == null)
                throw new ArgumentNullException("value");

            if (this.ContainsKey(valueMap.Key))
                throw new InvalidOperationException(string.Format("An item with key {0} has already been added.", valueMap.Key));

            this.valueMaps.Add(valueMap.Key, valueMap);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// 	<c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        private bool ContainsKey(string key)
        {
            return this.valueMaps.ContainsKey(key);
        }

        #endregion
    }
}