using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public abstract class SuperClassMap : ClassMap
    {
        #region Private Fields

        private ExtendedPropertiesMap extendedPropertiesMap;
        private readonly List<SubClassMap> subClassMaps;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the discriminator key.
        /// </summary>
        /// <value>The discriminator key.</value>
        public override string DiscriminatorKey { get; set; }

        /// <summary>
        /// Gets the extended properties map.
        /// </summary>
        /// <value>The extended properties map.</value>
        public override ExtendedPropertiesMap ExtendedPropertiesMap { get; set; }

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
        public virtual IEnumerable<SubClassMap> SubClassMaps
        {
            get { return this.subClassMaps; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SuperClassMap"/> class.
        /// </summary>
        /// <param name="type">Type of the entity.</param>
        public SuperClassMap(Type type)
            : base(type)
        {
            this.subClassMaps = new List<SubClassMap>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the sub class map.
        /// </summary>
        /// <param name="subClassMap">The sub class map.</param>
        public void AddSubClassMap(SubClassMap subClassMap)
        {
            if (subClassMap == null)
                throw new ArgumentNullException("subClassMap");

            this.subClassMaps.Add(subClassMap);
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