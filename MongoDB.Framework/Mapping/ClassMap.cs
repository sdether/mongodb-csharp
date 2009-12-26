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

        private readonly Dictionary<string, NestedClassMemberMap> nestedClassMemberMaps;
        private readonly Dictionary<string, ReferenceMemberMap> referenceMemberMaps;
        private readonly Dictionary<string, SimpleMemberMap> simpleMemberMaps;

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
        /// Gets the nestedClass member maps.
        /// </summary>
        /// <value>The nestedClass member maps.</value>
        public virtual IEnumerable<NestedClassMemberMap> NestedClassMemberMaps
        {
            get
            {
                foreach (var memberMap in this.nestedClassMemberMaps.Values)
                    yield return memberMap;
            }
        }

        /// <summary>
        /// Gets the member maps.
        /// </summary>
        /// <value>The member maps.</value>
        public virtual IEnumerable<ReferenceMemberMap> ReferenceMemberMaps
        {
            get
            {
                foreach (var memberMap in this.referenceMemberMaps.Values)
                    yield return memberMap;
            }
        }

        /// <summary>
        /// Gets the simple member maps.
        /// </summary>
        /// <value>The simple member maps.</value>
        public virtual IEnumerable<SimpleMemberMap> SimpleMemberMaps
        {
            get
            {
                foreach (var memberMap in this.simpleMemberMaps.Values)
                    yield return memberMap;
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

            this.nestedClassMemberMaps = new Dictionary<string, NestedClassMemberMap>();
            this.referenceMemberMaps = new Dictionary<string, ReferenceMemberMap>();
            this.simpleMemberMaps = new Dictionary<string, SimpleMemberMap>();
            this.Type = type;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the member map.
        /// </summary>
        /// <param name="memberMap">The member map.</param>
        public void AddNestedClassMemberMap(NestedClassMemberMap nestedClassMemberMap)
        {
            if (nestedClassMemberMap == null)
                throw new ArgumentNullException("value");

            if (this.ContainsKey(nestedClassMemberMap.Key))
                throw new InvalidOperationException(string.Format("An item with key {0} has already been added.", nestedClassMemberMap.Key));

            this.nestedClassMemberMaps.Add(nestedClassMemberMap.Key, nestedClassMemberMap);
        }

        /// <summary>
        /// Adds the member map.
        /// </summary>
        /// <param name="memberMap">The member map.</param>
        public void AddReferenceMemberMap(ReferenceMemberMap referenceMemberMap)
        {
            if (referenceMemberMap == null)
                throw new ArgumentNullException("value");

            if (this.ContainsKey(referenceMemberMap.Key))
                throw new InvalidOperationException(string.Format("An item with key {0} has already been added.", referenceMemberMap.Key));

            this.referenceMemberMaps.Add(referenceMemberMap.Key, referenceMemberMap);
        }

        /// <summary>
        /// Adds the member map.
        /// </summary>
        /// <param name="memberMap">The member map.</param>
        public void AddSimpleMemberMap(SimpleMemberMap simpleMemberMap)
        {
            if (simpleMemberMap == null)
                throw new ArgumentNullException("value");

            if (this.ContainsKey(simpleMemberMap.Key))
                throw new InvalidOperationException(string.Format("An item with key {0} has already been added.", simpleMemberMap.Key));

            this.simpleMemberMaps.Add(simpleMemberMap.Key, simpleMemberMap);
        }

        /// <summary>
        /// Gets the class map by discriminator.
        /// </summary>
        /// <param name="discriminator">The discriminator.</param>
        /// <returns></returns>
        public abstract ClassMap GetClassMapByDiscriminator(object discriminator);

        /// <summary>
        /// Gets the name of the member map from member.
        /// </summary>
        /// <param name="memberName">Name of the member.</param>
        /// <returns></returns>
        public MemberMap GetMemberMapFromMemberName(string memberName)
        {
            if (this.HasId && this.IdMap.MemberName == memberName)
                return this.IdMap;
            MemberMap memberMap = this.SimpleMemberMaps.FirstOrDefault(x => x.MemberName == memberName);
            if (memberMap != null)
                return memberMap;
            memberMap = this.NestedClassMemberMaps.FirstOrDefault(x => x.MemberName == memberName);
            if (memberMap != null)
                return memberMap;
            memberMap = this.ReferenceMemberMaps.FirstOrDefault(x => x.MemberName == memberName);
            if (memberMap != null)
                return memberMap;

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
            return this.nestedClassMemberMaps.ContainsKey(key)
                || this.referenceMemberMaps.ContainsKey(key)
                || this.simpleMemberMaps.ContainsKey(key);
        }

        #endregion
    }
}