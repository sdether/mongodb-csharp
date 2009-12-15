using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using MongoDB.Framework.Linq.Visitors;

namespace MongoDB.Framework.Configuration.Fluent
{
    public class FluentEntityMap<TEntity> : FluentDiscriminatedEntityMap<TEntity>
    {
        #region Public Properties

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public new EntityMap Instance
        {
            get { return (EntityMap)base.Instance; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentEntityMap&lt;TEntity&gt;"/> class.
        /// </summary>
        public FluentEntityMap()
            : base(new EntityMap(typeof(TEntity)))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentEntityMap&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="entityMap">The entity map.</param>
        public FluentEntityMap(EntityMap entityMap)
            : base(entityMap)
        { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Discriminates the by.
        /// </summary>
        /// <typeparam name="TDiscriminatorType">The type of the discriminator type.</typeparam>
        /// <param name="documentKey">The document key.</param>
        /// <returns></returns>
        public FluentEntityDiscriminatorMap<TDiscriminatorType> DiscriminateBy<TDiscriminatorType>(string documentKey)
        {
            this.Instance.DiscriminateDocumentKey = documentKey;
            return new FluentEntityDiscriminatorMap<TDiscriminatorType>(this.Instance);
        }

        /// <summary>
        /// Uses the extended properties.
        /// </summary>
        /// <param name="member">The member.</param>
        public void UseExtendedProperties(Expression<Func<TEntity, IDictionary<string, object>>> member)
        {
            if (member == null)
                throw new ArgumentNullException("member");

            var visitor = new MemberAccessMemberInfoVisitor();
            visitor.Visit(member);

            if (visitor.Members.Count() > 1)
                throw new NotSupportedException("Only top-level members are supported.");

            var memberInfo = visitor.Members.Single();
            this.Instance.ExtendedPropertiesMap = new ExtendedPropertiesMap(memberInfo);
        }

        #endregion

        #region Public Class : FluentEntityDiscriminatorMap

        public class FluentEntityDiscriminatorMap<TDiscriminator>
        {
            private EntityMap entityMap;

            /// <summary>
            /// Initializes a new instance of the <see cref="FluentEntityMap&lt;TEntity&gt;.FluentEntityDiscriminatorMap&lt;TDiscriminator&gt;"/> class.
            /// </summary>
            /// <param name="entityMap">The entity map.</param>
            public FluentEntityDiscriminatorMap(EntityMap entityMap)
            {
                this.entityMap = entityMap;
            }

            /// <summary>
            /// Entities the specified discriminating value.
            /// </summary>
            /// <typeparam name="TDiscriminatedEntity">The type of the discriminated entity.</typeparam>
            /// <param name="discriminatingValue">The discriminating value.</param>
            /// <param name="configure">The configure.</param>
            /// <returns></returns>
            public FluentEntityDiscriminatorMap<TDiscriminator> Entity<TDiscriminatedEntity>(TDiscriminator discriminatingValue, Action<FluentDiscriminatedEntityMap<TDiscriminatedEntity>> configure) where TDiscriminatedEntity : TEntity
            {
                var fluentDiscriminatedEntityMap = new FluentDiscriminatedEntityMap<TDiscriminatedEntity>();
                fluentDiscriminatedEntityMap.Instance.DiscriminatingValue = discriminatingValue;
                configure(fluentDiscriminatedEntityMap);
                this.entityMap.AddDiscriminatedEntityMap(fluentDiscriminatedEntityMap.Instance);
                return this;
            }

            /// <summary>
            /// Defaults the is.
            /// </summary>
            /// <param name="discriminatingRootValue">The discriminating root value.</param>
            /// <returns></returns>
            public FluentEntityDiscriminatorMap<TDiscriminator> DefaultIs(TDiscriminator discriminatingRootValue)
            {
                this.entityMap.DiscriminatingValue = discriminatingRootValue;
                return this;
            }
        }

        #endregion
    }
}