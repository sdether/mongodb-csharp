using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class NestedClassMap : SuperClassMap
    {
        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        /// <value>The name of the collection.</value>
        public override string CollectionName
        {
            get { throw new InvalidOperationException("NestedClasses cannot have collections."); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has indexes.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has indexes; otherwise, <c>false</c>.
        /// </value>
        public override bool HasIndexes
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the indexes.
        /// </summary>
        /// <value>The indexes.</value>
        public override IEnumerable<Index> Indexes
        {
            get { throw new InvalidOperationException("NestedClasses cannot have indexes."); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is root.
        /// </summary>
        /// <value><c>true</c> if this instance is root; otherwise, <c>false</c>.</value>
        public override bool IsRoot
        {
            get { return false; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NestedClassMap"/> class.
        /// </summary>
        /// <param name="type">ValueType of the entity.</param>
        /// <param name="idMap">The id map.</param>
        /// <param name="memberMaps">The member maps.</param>
        /// <param name="manyToOneMaps">The many to one maps.</param>
        /// <param name="discriminatorKey">The discriminator key.</param>
        /// <param name="discriminator">The discriminator.</param>
        /// <param name="subClassMaps">The sub class maps.</param>
        /// <param name="extendedPropertiesMap">The extended properties map.</param>
        public NestedClassMap(Type type, IdMap idMap, IEnumerable<MemberMap> memberMaps, IEnumerable<ManyToOneMap> manyToOneMaps, string discriminatorKey, object discriminator, IEnumerable<SubClassMap> subClassMaps, ExtendedPropertiesMap extendedPropertiesMap)
            : base(type, idMap, memberMaps, manyToOneMaps, discriminatorKey, discriminator, subClassMaps, extendedPropertiesMap)
        { }
    }
}