using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class RootClassMap : SuperClassMap
    {
        #region Private Fields

        private string collectionName;
        private IdMap idMap;
        private IEnumerable<Index> indexes;

	    #endregion

        #region Public Properties

        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        /// <value>The name of the collection.</value>
        public override string CollectionName
        {
            get { return this.collectionName; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has id.
        /// </summary>
        /// <value><c>true</c> if this instance has id; otherwise, <c>false</c>.</value>
        public override bool HasId
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has indexes.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has indexes; otherwise, <c>false</c>.
        /// </value>
        public override bool HasIndexes
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the id map.
        /// </summary>
        /// <value>The id map.</value>
        public override IdMap IdMap
        {
            get { return this.idMap; }
        }

        /// <summary>
        /// Gets the indexes.
        /// </summary>
        /// <value>The indexes.</value>
        public override IEnumerable<Index> Indexes
        {
            get { return this.indexes; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RootClassMap"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public RootClassMap(Type type, string collectionName, IdMap idMap, IEnumerable<MemberMap> memberMaps, string discriminatorKey, object discriminator, IEnumerable<SubClassMap> subClassMaps, ExtendedPropertiesMap extendedPropertiesMap, IEnumerable<Index> indexes)
            : base(type, memberMaps, discriminatorKey, discriminator, subClassMaps, extendedPropertiesMap)
        {
            if (collectionName == null)
                throw new ArgumentException("Cannot be null or empty.", "collectionName");
            if (idMap == null)
                throw new ArgumentNullException("idMap");

            this.collectionName = collectionName;
            this.idMap = idMap;
            this.indexes = indexes ?? Enumerable.Empty<Index>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IMapVisitor visitor)
        {
            visitor.ProcessRootClass(this);

            base.Accept(visitor);
        }

        #endregion
    }
}