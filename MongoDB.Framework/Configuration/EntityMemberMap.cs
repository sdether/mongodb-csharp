using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Configuration
{
    public class EntityMemberMap : MemberMap
    {
        /// <summary>
        /// Gets or sets the entity map.
        /// </summary>
        /// <value>The entity map.</value>
        public EntityMap EntityMap { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMemberMap"/> class.
        /// </summary>
        /// <param name="memberInfo">The member info.</param>
        /// <param name="entityMap">The entity map.</param>
        public EntityMemberMap(MemberInfo memberInfo, EntityMap entityMap)
            : base(memberInfo)
        {
            if (entityMap == null)
                throw new ArgumentNullException("entityMap");

            this.EntityMap = entityMap;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMemberMap"/> class.
        /// </summary>
        /// <param name="memberInfo">The member info.</param>
        /// <param name="documentKey">The document key.</param>
        /// <param name="entityMap">The entity map.</param>
        public EntityMemberMap(MemberInfo memberInfo, string documentKey, EntityMap entityMap)
            : base(memberInfo, documentKey)
        {
            if (entityMap == null)
                throw new ArgumentNullException("entityMap");

            this.EntityMap = entityMap;
        }
    }
}