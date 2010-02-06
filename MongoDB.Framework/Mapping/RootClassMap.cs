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
        private readonly List<Index> indexes;

	    #endregion

        #region Public Properties

        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        /// <value>The name of the collection.</value>
        public override string CollectionName
        {
            get { return this.collectionName; }
            internal set { this.collectionName = value; }
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
        /// Gets the indexes.
        /// </summary>
        /// <value>The indexes.</value>
        public override IEnumerable<Index> Indexes
        {
            get { return this.indexes; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is root.
        /// </summary>
        /// <value><c>true</c> if this instance is root; otherwise, <c>false</c>.</value>
        public override bool IsRoot
        {
            get { return true; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RootClassMap"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public RootClassMap(Type type)
            : base(type)
        {
            this.indexes = new List<Index>();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Adds the index.
        /// </summary>
        /// <param name="index">The index.</param>
        public void AddIndex(Index index)
        {
            if (index == null)
                throw new ArgumentNullException("index");

            this.indexes.Add(index);
        }

        /// <summary>
        /// Adds the indices.
        /// </summary>
        /// <param name="indices">The indices.</param>
        public void AddIndices(IEnumerable<Index> indices)
        {
            if (indices == null)
                throw new ArgumentNullException("indices");

            this.indexes.AddRange(indices);
        }

        #endregion
    }
}