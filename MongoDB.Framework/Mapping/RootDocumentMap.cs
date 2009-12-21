using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class RootDocumentMap : DocumentMap
    {
        #region Private Fields

        private readonly List<SubDocumentMap> subDocumentMaps;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the discriminator key.
        /// </summary>
        /// <value>The discriminator key.</value>
        public string DiscriminatorKey { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is polymorphic.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is polymorphic; otherwise, <c>false</c>.
        /// </value>
        public bool IsPolymorphic
        {
            get { return this.subDocumentMaps.Count > 0; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RootDocumentMap"/> class.
        /// </summary>
        /// <param name="mappingStore">The map store.</param>
        /// <param name="entityType">Type of the entity.</param>
        public RootDocumentMap(MappingStore mappingStore, Type entityType)
            : base(mappingStore, entityType)
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

        #endregion
    }
}