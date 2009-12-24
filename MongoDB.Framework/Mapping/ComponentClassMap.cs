using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class ComponentClassMap : SuperClassMap
    {
        #region Public Properties

        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        /// <value>The name of the collection.</value>
        public override string CollectionName
        {
            get { throw new NotSupportedException("Cannot get CollectionName from a ComponentClassMap.  Use the RootClassMap."); }
            set { throw new NotSupportedException("Cannot set CollectionName on a ComponentClassMap.  Use the RootClassMap."); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentClassMap"/> class.
        /// </summary>
        /// <param name="type">Type of the entity.</param>
        public ComponentClassMap(Type type)
            : base(type)
        { }

        #endregion
    }
}
