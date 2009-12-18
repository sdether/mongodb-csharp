using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Configuration
{
    public class ComponentMap : IMapVisitable
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public string DocumentKey { get; private set; }

        /// <summary>
        /// Gets or sets the entity map.
        /// </summary>
        /// <value>The entity map.</value>
        public EntityMap EntityMap { get; private set; }

        /// <summary>
        /// Gets or sets the getter.
        /// </summary>
        /// <value>The getter.</value>
        public Func<object, object> Getter { get; private set; }

        /// <summary>
        /// Gets or sets the name of the member.
        /// </summary>
        /// <value>The name of the member.</value>
        public string MemberName { get; private set; }

        /// <summary>
        /// Gets or sets the setter.
        /// </summary>
        /// <value>The setter.</value>
        public Action<object, object> Setter { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentMap"/> class.
        /// </summary>
        /// <param name="memberInfo">The member info.</param>
        /// <param name="entityMap">The entity map.</param>
        public ComponentMap(MemberInfo memberInfo, EntityMap entityMap)
        {
            if (entityMap == null)
                throw new ArgumentNullException("entityMap");

            this.DocumentKey = memberInfo.Name;
            this.EntityMap = entityMap;
            this.MemberName = memberInfo.Name;
            this.Getter = LateBoundReflection.GetGetter(memberInfo);
            this.Setter = LateBoundReflection.GetSetter(memberInfo);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMemberMap"/> class.
        /// </summary>
        /// <param name="memberInfo">The member info.</param>
        /// <param name="documentKey">The document key.</param>
        /// <param name="entityMap">The entity map.</param>
        public ComponentMap(MemberInfo memberInfo, string documentKey, EntityMap entityMap)
            : this(memberInfo, entityMap)
        {
            if (documentKey == null)
                throw new ArgumentException("Cannot be null or empty.", documentKey);

            this.DocumentKey = documentKey;
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public virtual void Accept(IMapVisitor visitor)
        {
            visitor.VisitComponentMap(this);
        }
    }
}