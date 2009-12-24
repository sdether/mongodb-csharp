using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping
{
    public abstract class ClassMap : Map
    {
        #region Private Fields

        private readonly Dictionary<string, NestedClassValueMap> nestedClassValueMaps;
        private readonly Dictionary<string, ReferenceValueMap> referenceValueMaps;
        private readonly Dictionary<string, SimpleValueMap> simpleValueMaps;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        /// <value>The name of the collection.</value>
        public abstract string CollectionName { get; set; }

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
        /// Gets a value indicating whether this instance has id.
        /// </summary>
        /// <value><c>true</c> if this instance has id; otherwise, <c>false</c>.</value>
        public bool HasId
        {
            get { return this.IdMap != null; }
        }

        /// <summary>
        /// Gets the id map.
        /// </summary>
        /// <value>The id map.</value>
        public virtual IdMap IdMap
        {
            get { return null; }
            set { throw new NotSupportedException("IdMap can only be set on a CollectionMap."); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is polymorphic.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is polymorphic; otherwise, <c>false</c>.
        /// </value>
        public abstract bool IsPolymorphic { get; }

        /// <summary>
        /// Gets the nestedClass value maps.
        /// </summary>
        /// <value>The nestedClass value maps.</value>
        public virtual IEnumerable<NestedClassValueMap> NestedClassValueMaps
        {
            get
            {
                foreach (var valueMap in this.nestedClassValueMaps.Values)
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

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassMap"/> class.
        /// </summary>
        /// <param name="metaDataStore">The meta data store.</param>
        /// <param name="type">Type of the entity.</param>
        public ClassMap(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            this.nestedClassValueMaps = new Dictionary<string, NestedClassValueMap>();
            this.referenceValueMaps = new Dictionary<string, ReferenceValueMap>();
            this.simpleValueMaps = new Dictionary<string, SimpleValueMap>();
            this.Type = type;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the value map.
        /// </summary>
        /// <param name="valueMap">The value map.</param>
        public void AddNestedClassValueMap(NestedClassValueMap nestedClassValueMap)
        {
            if (nestedClassValueMap == null)
                throw new ArgumentNullException("value");

            if (this.ContainsKey(nestedClassValueMap.Key))
                throw new InvalidOperationException(string.Format("An item with key {0} has already been added.", nestedClassValueMap.Key));

            this.nestedClassValueMaps.Add(nestedClassValueMap.Key, nestedClassValueMap);
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

        /// <summary>
        /// Gets the class map by discriminator.
        /// </summary>
        /// <param name="discriminator">The discriminator.</param>
        /// <returns></returns>
        public abstract ClassMap GetClassMapByDiscriminator(object discriminator);

        /// <summary>
        /// Gets the name of the value map from member.
        /// </summary>
        /// <param name="memberName">Name of the member.</param>
        /// <returns></returns>
        public ValueMap GetValueMapFromMemberName(string memberName)
        {
            if (this.HasId && this.IdMap.MemberName == memberName)
                return this.IdMap;
            ValueMap valueMap = this.SimpleValueMaps.FirstOrDefault(x => x.MemberName == memberName);
            if (valueMap != null)
                return valueMap;
            valueMap = this.NestedClassValueMaps.FirstOrDefault(x => x.MemberName == memberName);
            if (valueMap != null)
                return valueMap;
            valueMap = this.ReferenceValueMaps.FirstOrDefault(x => x.MemberName == memberName);
            if (valueMap != null)
                return valueMap;

            throw new UnmappedMemberException(string.Format("The member {0} has not been mapped.", memberName));
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
            return this.nestedClassValueMaps.ContainsKey(key)
                || this.referenceValueMaps.ContainsKey(key)
                || this.simpleValueMaps.ContainsKey(key);
        }

        #endregion
    }
}