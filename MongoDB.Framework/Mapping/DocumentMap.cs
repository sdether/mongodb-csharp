using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public abstract class DocumentMap : Map
    {
        #region Private Fields

        private readonly Dictionary<string, NestedDocumentValueMap> nestedDocumentValueMaps;
        private readonly Dictionary<string, ReferenceValueMap> referenceValueMaps;
        private readonly Dictionary<string, SimpleValueMap> simpleValueMaps;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the discriminator.
        /// </summary>
        /// <value>The discriminator.</value>
        public object Discriminator { get; set; }

        /// <summary>
        /// Gets or sets the discriminator key.
        /// </summary>
        /// <value>The discriminator key.</value>
        public abstract string DiscriminatorKey { get; set; }

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <value>The type of the entity.</value>
        public Type EntityType { get; private set; }

        /// <summary>
        /// Gets the extended properties map.
        /// </summary>
        /// <value>The extended properties map.</value>
        public abstract ExtendedPropertiesMap ExtendedPropertiesMap { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has extended properties.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has extended properties; otherwise, <c>false</c>.
        /// </value>
        public bool HasExtendedProperties
        {
            get { return this.ExtendedPropertiesMap != null; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is polymorphic.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is polymorphic; otherwise, <c>false</c>.
        /// </value>
        public abstract bool IsPolymorphic { get; }

        /// <summary>
        /// Gets the value maps.
        /// </summary>
        /// <value>The value maps.</value>
        public virtual IEnumerable<NestedDocumentValueMap> NestedDocumentValueMaps
        {
            get
            {
                foreach (var valueMap in this.nestedDocumentValueMaps.Values)
                    yield return valueMap;
            }
        }

        /// <summary>
        /// Gets the value maps.
        /// </summary>
        /// <value>The value maps.</value>
        public virtual IEnumerable<ReferenceValueMap> ReferenceValueMaps
        {
            get
            {
                foreach (var valueMap in this.referenceValueMaps.Values)
                    yield return valueMap;
            }
        }

        /// <summary>
        /// Gets the simple value maps.
        /// </summary>
        /// <value>The simple value maps.</value>
        public virtual IEnumerable<SimpleValueMap> SimpleValueMaps
        {
            get
            {
                foreach (var valueMap in this.simpleValueMaps.Values)
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
        public DocumentMap(Type entityType)
        {
            if (entityType == null)
                throw new ArgumentNullException("entityType");

            this.EntityType = entityType;
            this.nestedDocumentValueMaps = new Dictionary<string, NestedDocumentValueMap>();
            this.referenceValueMaps = new Dictionary<string, ReferenceValueMap>();
            this.simpleValueMaps = new Dictionary<string, SimpleValueMap>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the value map.
        /// </summary>
        /// <param name="valueMap">The value map.</param>
        public void AddNestedDocumentValueMap(NestedDocumentValueMap nestedDocumentValueMap)
        {
            if (nestedDocumentValueMap == null)
                throw new ArgumentNullException("value");

            if (this.ContainsKey(nestedDocumentValueMap.Key))
                throw new InvalidOperationException(string.Format("An item with key {0} has already been added.", nestedDocumentValueMap.Key));

            this.nestedDocumentValueMaps.Add(nestedDocumentValueMap.Key, nestedDocumentValueMap);
        }

        /// <summary>
        /// Adds the value map.
        /// </summary>
        /// <param name="valueMap">The value map.</param>
        public void AddReferenceValueMap(ReferenceValueMap referenceValueMap)
        {
            if (referenceValueMap == null)
                throw new ArgumentNullException("value");

            if (this.ContainsKey(referenceValueMap.Key))
                throw new InvalidOperationException(string.Format("An item with key {0} has already been added.", referenceValueMap.Key));

            this.referenceValueMaps.Add(referenceValueMap.Key, referenceValueMap);
        }

        /// <summary>
        /// Adds the value map.
        /// </summary>
        /// <param name="valueMap">The value map.</param>
        public void AddSimpleValueMap(SimpleValueMap simpleValueMap)
        {
            if (simpleValueMap == null)
                throw new ArgumentNullException("value");

            if (this.ContainsKey(simpleValueMap.Key))
                throw new InvalidOperationException(string.Format("An item with key {0} has already been added.", simpleValueMap.Key));

            this.simpleValueMaps.Add(simpleValueMap.Key, simpleValueMap);
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
            return this.nestedDocumentValueMaps.ContainsKey(key)
                || this.referenceValueMaps.ContainsKey(key)
                || this.simpleValueMaps.ContainsKey(key);
        }

        #endregion
    }
}