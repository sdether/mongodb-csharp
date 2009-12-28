using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class SubClassMap : ClassMap
    {
        #region Private Fields

        private string collectionName;
        private string discriminatorKey;
        private ExtendedPropertiesMap extendedPropertiesMap;
        private MemberMap idMap;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        /// <value>The name of the collection.</value>
        public override string CollectionName
        {
            get { return this.collectionName; }
        }

        /// <summary>
        /// Gets or sets the discriminator key.
        /// </summary>
        /// <value>The discriminator key.</value>
        public override string DiscriminatorKey
        {
            get { return this.discriminatorKey; }
        }

        /// <summary>
        /// Gets the extended properties map.
        /// </summary>
        /// <value>The extended properties map.</value>
        public override ExtendedPropertiesMap ExtendedPropertiesMap
        {
            get { return this.extendedPropertiesMap; }
        }

        /// <summary>
        /// Gets the id map.
        /// </summary>
        /// <value>The id map.</value>
        public override MemberMap IdMap
        {
            get { return this.idMap; }
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
        public SubClassMap(Type type, string collectionName, MemberMap idMap, IEnumerable<MemberMap> memberMaps, string discriminatorKey, object discriminator, ExtendedPropertiesMap extendedPropertiesMap)
            : base(type, memberMaps, discriminator)
        {
            this.collectionName = collectionName;
            this.idMap = idMap;
            this.discriminatorKey = discriminatorKey;
            this.extendedPropertiesMap = extendedPropertiesMap;
        }

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
    }
}