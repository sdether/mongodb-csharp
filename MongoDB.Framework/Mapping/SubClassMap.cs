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
            set { throw new NotSupportedException("Cannot set CollectionName on a SubClassMap.  Use the CollectionMap."); }
        }

        /// <summary>
        /// Gets or sets the discriminator key.
        /// </summary>
        /// <value>The discriminator key.</value>
        public override string DiscriminatorKey
        {
            get { return this.SuperClassMap.DiscriminatorKey; }
            set { throw new NotSupportedException("Cannot set DiscriminatorKey on a SubClassMap.  Use the RootClassMap."); }
        }

        /// <summary>
        /// Gets the extended properties map.
        /// </summary>
        /// <value>The extended properties map.</value>
        public override ExtendedPropertiesMap ExtendedPropertiesMap
        {
            get { return this.SuperClassMap.ExtendedPropertiesMap; }
            set { throw new NotSupportedException("Cannot set ExtendedPropertiesMap on a SubClassMap.  Use the RootClassMap."); }
        }

        /// <summary>
        /// Gets the id map.
        /// </summary>
        /// <value>The id map.</value>
        public override IdMap IdMap
        {
            get { return this.SuperClassMap.IdMap; }
            set { base.IdMap = value; } //should throw...
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
                return base.MemberMaps
                    .Concat(this.SuperClassMap.MemberMaps);
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
        /// <param name="classMap">The class map.</param>
        public SubClassMap(Type type, SuperClassMap superClassMap)
            : base(type)
        {
            if (superClassMap == null)
                throw new ArgumentNullException("superClassMap");

            this.SuperClassMap = superClassMap;
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

        #region Protected Methods

        /// <summary>
        /// Creates the owner.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        protected override Type GetConcreteType(MongoDB.Driver.Document document)
        {
            return this.Type;
        }

        #endregion
    }
}