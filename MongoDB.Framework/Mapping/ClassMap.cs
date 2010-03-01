using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class ClassMap : ClassMapBase
    {
        #region Private Fields

        private string collectionName;
        private readonly List<Index> indexes;
        private string discriminatorKey;
        private object discriminator;
        private ExtendedPropertiesMap extendedPropertiesMap;
        private IdMap idMap;
        private readonly List<SubClassMap> subClassMaps;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        /// <value>The name of the collection.</value>
        public override string CollectionName
        {
            get { return this.collectionName; }
            internal set { this.collectionName = value; }
        }

        /// <summary>
        /// Gets or sets the discriminator key.
        /// </summary>
        /// <value>The discriminator key.</value>
        public override string DiscriminatorKey
        {
            get { return this.discriminatorKey; }
            internal set { this.discriminatorKey = value; }
        }

        /// <summary>
        /// Gets the extended properties map.
        /// </summary>
        /// <value>The extended properties map.</value>
        public override ExtendedPropertiesMap ExtendedPropertiesMap
        {
            get { return this.extendedPropertiesMap; }
            internal set { this.extendedPropertiesMap = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has extended properties.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has extended properties; otherwise, <c>false</c>.
        /// </value>
        public override bool HasExtendedProperties
        {
            get { return this.extendedPropertiesMap != null; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has id.
        /// </summary>
        /// <value><c>true</c> if this instance has id; otherwise, <c>false</c>.</value>
        public override bool HasId
        {
            get { return this.idMap != null; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has indexes.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has indexes; otherwise, <c>false</c>.
        /// </value>
        public override bool HasIndexes
        {
            get { return this.indexes.Any(); }
        }

        /// <summary>
        /// Gets the indexes.
        /// </summary>
        /// <value>The indexes.</value>
        public override IEnumerable<Index> Indexes
        {
            get { return this.indexes; }
        }

        /// <summary>
        /// Gets the id map.
        /// </summary>
        /// <value>The id map.</value>
        public override IdMap IdMap
        {
            get { return this.idMap; }
            internal set { this.idMap = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is polymorphic.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is polymorphic; otherwise, <c>false</c>.
        /// </value>
        ///
        public override bool IsPolymorphic
        {
            get { return this.subClassMaps.Count > 0; }
        }

        /// <summary>
        /// Gets the sub class maps.
        /// </summary>
        /// <value>The sub class maps.</value>
        public IEnumerable<SubClassMap> SubClassMaps
        {
            get { return this.subClassMaps; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SuperClassMap"/> class.
        /// </summary>
        /// <param name="type">ValueType of the entity.</param>
        public ClassMap(Type type)
            : base(type)
        {
            this.indexes = new List<Index>();
            this.subClassMaps = new List<SubClassMap>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the class map by discriminator.
        /// </summary>
        /// <param name="discriminator">The discriminator.</param>
        /// <returns></returns>
        public override ClassMapBase GetClassMapByDiscriminator(object discriminator)
        {
            if (this.Discriminator == null)
            {
                if (discriminator == null)
                    return this;
            }
            else if (this.Discriminator.Equals(discriminator))
                return this;

            foreach (var subClassMap in this.subClassMaps)
                if (subClassMap.Discriminator.Equals(discriminator))
                    return subClassMap;

            throw new UnmappedDiscriminatorException(string.Format("The discriminator {0} has not been mapped.", discriminator));
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Adds the index.
        /// </summary>
        /// <param name="index">The index.</param>
        public void AddIndex(Index index)
        {
            if (index == null)
                throw new ArgumentNullException("index");

            this.indexes.Add(index);
        }

        /// <summary>
        /// Adds the indices.
        /// </summary>
        /// <param name="indices">The indices.</param>
        public void AddIndices(IEnumerable<Index> indices)
        {
            if (indices == null)
                throw new ArgumentNullException("indices");

            this.indexes.AddRange(indices);
        }

        /// <summary>
        /// Adds the sub class map.
        /// </summary>
        /// <param name="subClassMap">The sub class map.</param>
        internal void AddSubClassMap(SubClassMap subClassMap)
        {
            if (subClassMap == null)
                throw new ArgumentNullException("subClassMap");

            this.subClassMaps.Add(subClassMap);
            subClassMap.SuperClassMap = this;
        }

        /// <summary>
        /// Adds the sub class maps.
        /// </summary>
        /// <param name="subClassMaps">The sub class maps.</param>
        internal void AddSubClassMaps(IEnumerable<SubClassMap> subClassMaps)
        {
            if (subClassMaps == null)
                throw new ArgumentNullException("subClassMaps");

            foreach (var subClassMap in subClassMaps)
                this.AddSubClassMap(subClassMap);
        }

        #endregion
    }
}