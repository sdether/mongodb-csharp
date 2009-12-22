using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class CollectionMap : RootDocumentMap
    {
        #region Private Fields

        private IdMap idMap;

	    #endregion

        #region Public Properties

        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        /// <value>The name of the collection.</value>
        public override string CollectionName { get; set; }

        /// <summary>
        /// Gets or sets the id map.
        /// </summary>
        /// <value>The id map.</value>
        public override IdMap IdMap
        {
            get { return this.idMap; }
            set { this.idMap = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionMap"/> class.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        public CollectionMap(Type entityType)
            : base(entityType)
        { }

        #endregion
    }
}