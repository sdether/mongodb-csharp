using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class ReferenceValueMap : ValueMap
    {
        /// <summary>
        /// Gets or sets the collection map.
        /// </summary>
        /// <value>The collection map.</value>
        public CollectionMap CollectionMap { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceValueMap"/> class.
        /// </summary>
        /// <param name="metaDataStore">The meta data store.</param>
        /// <param name="key">The key.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="memberType">Type of the member.</param>
        /// <param name="memberGetter">The member getter.</param>
        /// <param name="memberSetter">The member setter.</param>
        /// <param name="collectionMap">The collection map.</param>
        public ReferenceValueMap(MappingStore metaDataStore, string key, string memberName, Type memberType, Func<object, object> memberGetter, Action<object, object> memberSetter, CollectionMap collectionMap)
            : base(metaDataStore, key, memberName, memberType, memberGetter, memberSetter)
        {
            if (collectionMap == null)
                throw new ArgumentNullException("collectionMap");

            this.CollectionMap = collectionMap;
        }
    }
}