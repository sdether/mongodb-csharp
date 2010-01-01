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
        /// Gets the id map.
        /// </summary>
        /// <value>The id map.</value>
        public override IdMap IdMap
        {
            get { return this.SuperClassMap.IdMap; }
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
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="idMap">The id map.</param>
        /// <param name="memberMaps">The member maps.</param>
        /// <param name="discriminatorKey">The discriminator key.</param>
        /// <param name="discriminator">The discriminator.</param>
        /// <param name="extendedPropertiesMap">The extended properties map.</param>
        public SubClassMap(Type type, IEnumerable<MemberMap> memberMaps, object discriminator)
            : base(type, memberMaps, discriminator)
        {  }

        #endregion

        #region Public Methods

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IMapVisitor visitor)
        {
            visitor.ProcessSubClass(this);

            base.Accept(visitor);
        }

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