using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class SubDocumentMap : DocumentMap
    {
        /// <summary>
        /// Gets the root document map.
        /// </summary>
        /// <value>The root document map.</value>
        public RootDocumentMap RootDocumentMap { get; private set; }

        /// <summary>
        /// Gets the simple value maps.
        /// </summary>
        /// <value>The simple value maps.</value>
        public virtual IEnumerable<NestedDocumentValueMap> NestedDocumentValueMaps
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
        public virtual IEnumerable<ReferenceValueMap> ReferenceValueMaps
        {
            get
            {
                return base.ReferenceValueMaps
                    .Concat(this.RootDocumentMap.ReferenceValueMaps);
            }
        }

        /// <summary>
        /// Gets the simple value maps.
        /// </summary>
        /// <value>The simple value maps.</value>
        public virtual IEnumerable<SimpleValueMap> SimpleValueMaps
        {
            get
            {
                return base.SimpleValueMaps
                    .Concat(this.RootDocumentMap.SimpleValueMaps);
            }
        }

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
    }
}