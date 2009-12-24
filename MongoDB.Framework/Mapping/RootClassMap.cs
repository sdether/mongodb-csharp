using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class RootClassMap : SuperClassMap
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
        /// Initializes a new instance of the <see cref="RootClassMap"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public RootClassMap(Type type)
            : base(type)
        { }

        #endregion
    }
}