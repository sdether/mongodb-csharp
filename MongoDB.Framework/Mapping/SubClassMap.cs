using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class SubClassMap : ClassMap
    {
        #region Public Properties

        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        /// <value>The name of the collection.</value>
        public override string CollectionName
        {
            get { return this.SuperClassMap.CollectionName; }
        }

        /// <summary>
        /// Gets or sets the discriminator key.
        /// </summary>
        /// <value>The discriminator key.</value>
        public override string DiscriminatorKey
        {
            get { return this.SuperClassMap.DiscriminatorKey; }
        }

        /// <summary>
        /// Gets the extended properties map.
        /// </summary>
        /// <value>The extended properties map.</value>
        public override ExtendedPropertiesMap ExtendedPropertiesMap
        {
            get { return this.SuperClassMap.ExtendedPropertiesMap; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has extended properties.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has extended properties; otherwise, <c>false</c>.
        /// </value>
        public override bool HasExtendedProperties
        {
            get { return this.SuperClassMap.HasExtendedProperties; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has id.
        /// </summary>
        /// <value><c>true</c> if this instance has id; otherwise, <c>false</c>.</value>
        public override bool HasId
        {
            get { return this.SuperClassMap.HasId; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has indexes.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has indexes; otherwise, <c>false</c>.
        /// </value>
        public override bool HasIndexes
        {
            get { return this.SuperClassMap.HasIndexes; }
        }

        /// <summary>
        /// Gets the id map.
        /// </summary>
        /// <value>The id map.</value>
        public override IdMap IdMap
        {
            get { return this.SuperClassMap.IdMap; }
        }

        /// <summary>
        /// Gets the indexes.
        /// </summary>
        /// <value>The indexes.</value>
        public override IEnumerable<Index> Indexes
        {
            get { return this.SuperClassMap.Indexes; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is polymorhic.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is polymorhic; otherwise, <c>false</c>.
        /// </value>
        public override bool IsPolymorphic
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is root.
        /// </summary>
        /// <value><c>true</c> if this instance is root; otherwise, <c>false</c>.</value>
        public override bool IsRoot
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the member maps.
        /// </summary>
        /// <value>The simple member maps.</value>
        public override IEnumerable<MemberMap> MemberMaps
        {
            get
            {
                return this.SuperClassMap.MemberMaps
                    .Concat(base.MemberMaps);
            }
        }

        /// <summary>
        /// Gets the super class map.
        /// </summary>
        /// <value>The super class map.</value>
        public SuperClassMap SuperClassMap { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SubClassMap"/> class.
        /// </summary>
        /// <param name="type">ValueType of the entity.</param>
        /// <param name="memberMaps">The member maps.</param>
        /// <param name="discriminator">The discriminator.</param>
        public SubClassMap(Type type, IEnumerable<MemberMap> memberMaps, object discriminator)
            : base(type, memberMaps,  discriminator)
        {  }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the class map by discriminator.
        /// </summary>
        /// <param name="discriminator">The discriminator.</param>
        /// <returns></returns>
        public override ClassMap GetClassMapByDiscriminator(object discriminator)
        {
            if (!this.Discriminator.Equals(discriminator))
                throw new InvalidOperationException(string.Format("The discriminator specified does not belong to the entity {0}.", this.Type));

            return this;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Sets the super class.
        /// </summary>
        /// <param name="superClassMap">The super class map.</param>
        internal void SetSuperClass(SuperClassMap superClassMap)
        {
            this.SuperClassMap = superClassMap;
        }

        #endregion
    }
}