using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class CollectionMap : RootDocumentMap
    {
        #region Public Properties

        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        /// <value>The name of the collection.</value>
        public string CollectionName { get; private set; }

        /// <summary>
        /// Gets the id map.
        /// </summary>
        /// <value>The id map.</value>
        public ValueMap IdMap { get; private set; }

        /// <summary>
        /// Gets the value maps.
        /// </summary>
        /// <value>The value maps.</value>
        public override IEnumerable<ValueMap> ValueMaps
        {
            get
            {
                return base.ValueMaps.Concat(new[] { this.IdMap });
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionMap"/> class.
        /// </summary>
        /// <param name="mappingStore">The map store.</param>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="idMap">The id map.</param>
        public CollectionMap(MappingStore mappingStore, Type entityType, string collectionName, ValueMap idMap)
            : base(mappingStore, entityType)
        {
            if (collectionName == null)
                throw new ArgumentException("Cannot be null or empty.", "collectionName");
            if (idMap == null)
                throw new ArgumentNullException("idMap");

            this.CollectionName = collectionName;
            this.IdMap = idMap;
        }

        #endregion
    }
}