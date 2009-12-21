using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class SubDocumentMap : DocumentMap
    {
        #region Public Properties

        public override string DiscriminatorKey
        {
            get { return this.RootDocumentMap.DiscriminatorKey; }
            set { throw new NotSupportedException("Cannot set DiscriminatorKey on a SubDocumentMap.  Use the RootDocumentMap."); }
        }

        /// <summary>
        /// Gets the extended properties map.
        /// </summary>
        /// <value>The extended properties map.</value>
        public override ExtendedPropertiesMap ExtendedPropertiesMap
        {
            get { return this.RootDocumentMap.ExtendedPropertiesMap; }
            set { throw new NotSupportedException("Cannot set ExtendedPropertiesMap on a SubDocumentMap.  Use the RootDocumentMap."); }
        }

        /// <summary>
        /// Gets the id map.
        /// </summary>
        /// <value>The id map.</value>
        public override IdMap IdMap
        {
            get { return this.RootDocumentMap.IdMap; }
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
        /// Gets the simple value maps.
        /// </summary>
        /// <value>The simple value maps.</value>
        public override IEnumerable<NestedDocumentValueMap> NestedDocumentValueMaps
        {
            get
            {
                return base.NestedDocumentValueMaps
                    .Concat(this.RootDocumentMap.NestedDocumentValueMaps);
            }
        }

        /// <summary>
        /// Gets the simple value maps.
        /// </summary>
        /// <value>The simple value maps.</value>
        public override IEnumerable<ReferenceValueMap> ReferenceValueMaps
        {
            get
            {
                return base.ReferenceValueMaps
                    .Concat(this.RootDocumentMap.ReferenceValueMaps);
            }
        }

        /// <summary>
        /// Gets the root document map.
        /// </summary>
        /// <value>The root document map.</value>
        public RootDocumentMap RootDocumentMap { get; private set; }

        /// <summary>
        /// Gets the simple value maps.
        /// </summary>
        /// <value>The simple value maps.</value>
        public override IEnumerable<SimpleValueMap> SimpleValueMaps
        {
            get
            {
                return base.SimpleValueMaps
                    .Concat(this.RootDocumentMap.SimpleValueMaps);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SubDocumentMap"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="rootDocumentMap">The root document map.</param>
        public SubDocumentMap(Type entityType, RootDocumentMap rootDocumentMap)
            : base(entityType)
        {
            if (rootDocumentMap == null)
                throw new ArgumentNullException("parentDocumentMap");

            this.RootDocumentMap = rootDocumentMap;
        }

        #endregion
    }
}