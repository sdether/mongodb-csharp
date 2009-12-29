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
        /// Gets the id map.
        /// </summary>
        /// <value>The id map.</value>
        public override IdMap IdMap
        {
            get { return this.idMap; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RootClassMap"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public RootClassMap(Type type, string collectionName, IdMap idMap, IEnumerable<MemberMap> memberMaps, string discriminatorKey, object discriminator, IEnumerable<SubClassMap> subClassMaps, ExtendedPropertiesMap extendedPropertiesMap)
            : base(type, memberMaps, discriminatorKey, discriminator, subClassMaps, extendedPropertiesMap)
        {
            if (collectionName == null)
                throw new ArgumentException("Cannot be null or empty.", "collectionName");
            if (idMap == null)
                throw new ArgumentNullException("idMap");

            this.collectionName = collectionName;
            this.idMap = idMap;
        }

        #endregion
    }
}