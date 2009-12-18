using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Linq.Visitors;

namespace MongoDB.Framework.Configuration
{
    public class RootEntityMap : EntityMap
    {
        #region Private Fields

        private List<Index> indexes;
        
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        /// <value>The name of the collection.</value>
        public string CollectionName { get; set; }

        /// <summary>
        /// Gets or sets the id map.
        /// </summary>
        /// <value>The id map.</value>
        public IdMap IdMap{ get; set; }

        /// <summary>
        /// Gets the indexes.
        /// </summary>
        /// <value>The indexes.</value>
        public IEnumerable<Index> Indexes
        {
            get { return this.indexes; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMap"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public RootEntityMap(Type type)
            : base(type)
        {
            this.CollectionName = type.Name;
            this.indexes = new List<Index>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMap"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="collectionName">Name of the collection.</param>
        public RootEntityMap(Type type, string collectionName)
            : this(type)
        {
            this.CollectionName = collectionName;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IMapVisitor visitor)
        {
            visitor.VisitRootEntityMap(this);
            base.Accept(visitor);
        }

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

        #endregion
    }
}