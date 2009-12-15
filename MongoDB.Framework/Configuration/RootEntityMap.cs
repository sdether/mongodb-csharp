using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Visitors;

namespace MongoDB.Framework.Configuration
{
    public class RootEntityMap : EntityMap
    {
        #region Public Properties

        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        /// <value>The name of the collection.</value>
        public string CollectionName { get; set; }

        /// <summary>
        /// Gets or sets the id member map.
        /// </summary>
        /// <value>The id member map.</value>
        public MemberMap IdMemberMap { get; set; }

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
    }
}