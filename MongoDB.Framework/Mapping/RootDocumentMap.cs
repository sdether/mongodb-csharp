using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class RootDocumentMap : DocumentMap
    {
        #region Private Fields

        private ExtendedPropertiesMap extendedPropertiesMap;
        private readonly List<SubDocumentMap> subDocumentMaps;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        /// <value>The name of the collection.</value>
        public override string CollectionName
        {
            get { throw new NotSupportedException("Cannot get CollectionName from a RootDocumentMap.  Use the CollectionMap."); }
            set { throw new NotSupportedException("Cannot set CollectionName on a RootDocumentMap.  Use the CollectionMap."); }
        }

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
            get { return this.subDocumentMaps.Count > 0; }
        }

        /// <summary>
        /// Gets the sub document maps.
        /// </summary>
        /// <value>The sub document maps.</value>
        public virtual IEnumerable<SubDocumentMap> SubDocumentMaps
        {
            get { return this.subDocumentMaps; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RootDocumentMap"/> class.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        public RootDocumentMap(Type entityType)
            : base(entityType)
        {
            this.subDocumentMaps = new List<SubDocumentMap>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the sub document map.
        /// </summary>
        /// <param name="subDocumentMap">The sub document map.</param>
        public void AddSubDocumentMap(SubDocumentMap subDocumentMap)
        {
            if (subDocumentMap == null)
                throw new ArgumentNullException("subDocumentMap");

            this.subDocumentMaps.Add(subDocumentMap);
        }

        /// <summary>
        /// Gets the document map by discriminator.
        /// </summary>
        /// <param name="discriminator">The discriminator.</param>
        /// <returns></returns>
        public override DocumentMap GetDocumentMapByDiscriminator(object discriminator)
        {
            if (this.Discriminator == null)
            {
                if (discriminator == null)
                    return this;
            }
            else if (this.Discriminator.Equals(discriminator))
                return this;

            foreach (var subDocumentMap in this.subDocumentMaps)
                if (subDocumentMap.Discriminator.Equals(discriminator))
                    return subDocumentMap;

            throw new UnmappedDiscriminatorException(string.Format("The discriminator {0} has not been mapped.", discriminator));
        }

        #endregion
    }
}