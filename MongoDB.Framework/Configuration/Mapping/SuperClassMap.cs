using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping
{
    public abstract class SuperClassMap : ClassMap
    {
        #region Private Fields

        private string discriminatorKey;
        private object discriminator;
        private ExtendedPropertiesMap extendedPropertiesMap;
        private readonly List<SubClassMap> subClassMaps;

        #endregion

        #region Public Properties

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
        /// <param name="memberMaps">The member maps.</param>
        /// <param name="manyToOneMaps">The many to one maps.</param>
        /// <param name="discriminatorKey">The discriminator key.</param>
        /// <param name="discriminator">The discriminator.</param>
        /// <param name="subClassMaps">The sub class maps.</param>
        /// <param name="extendedPropertiesMap">The extended properties map.</param>
        protected SuperClassMap(Type type, IEnumerable<MemberMap> memberMaps, IEnumerable<ManyToOneMap> manyToOneMaps, string discriminatorKey, object discriminator, IEnumerable<SubClassMap> subClassMaps, ExtendedPropertiesMap extendedPropertiesMap)
            : base(type, memberMaps, manyToOneMaps, discriminator)
        {
            this.discriminatorKey = discriminatorKey;
            this.extendedPropertiesMap = extendedPropertiesMap;
            this.subClassMaps = new List<SubClassMap>(subClassMaps ?? new SubClassMap[0]);
            foreach (var subClassMap in this.subClassMaps)
                subClassMap.SetSuperClass(this);
        }

        #endregion

        #region Public Methods

        public override void Accept(IMapVisitor visitor)
        {
            visitor.ProcessSuperClass(this);

            base.Accept(visitor);
        }

        /// <summary>
        /// Gets the class map by discriminator.
        /// </summary>
        /// <param name="discriminator">The discriminator.</param>
        /// <returns></returns>
        public override ClassMap GetClassMapByDiscriminator(object discriminator)
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
    }
}