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
        /// Gets the value maps.
        /// </summary>
        /// <value>The value maps.</value>
        public virtual IEnumerable<ValueMap> ValueMaps
        {
            get
            {
                return base.ValueMaps.Concat(this.RootDocumentMap.ValueMaps);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubDocumentMap"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="rootDocumentMap">The root document map.</param>
        public SubDocumentMap(MappingStore mappingStore, Type entityType, RootDocumentMap rootDocumentMap)
            : base(mappingStore, entityType)
        {
            if (rootDocumentMap == null)
                throw new ArgumentNullException("parentDocumentMap");

            this.RootDocumentMap = rootDocumentMap;
        }
    }
}