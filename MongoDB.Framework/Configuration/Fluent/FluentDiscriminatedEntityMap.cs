using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Linq.Visitors;

namespace MongoDB.Framework.Configuration.Fluent
{
    public class FluentDiscriminatedEntityMap<TDiscriminatedEntity>
    {
        #region Public Properties

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public DiscriminatedEntityMap Instance { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentDiscriminatedEntityMap&lt;TDiscriminatedEntity&gt;"/> class.
        /// </summary>
        public FluentDiscriminatedEntityMap()
            : this(new DiscriminatedEntityMap(typeof(TDiscriminatedEntity)))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentDiscriminatedEntityMap&lt;TDiscriminatedEntity&gt;"/> class.
        /// </summary>
        /// <param name="discriminatedEntityMap">The discriminated entity map.</param>
        public FluentDiscriminatedEntityMap(DiscriminatedEntityMap discriminatedEntityMap)
        {
            if (discriminatedEntityMap == null)
                throw new ArgumentNullException("discriminatedEntityMap");

            this.Instance = discriminatedEntityMap;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Entities the specified member.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="member">The member.</param>
        /// <param name="configure">The configure.</param>
        public void Entity<TEntity>(MemberInfo member, Action<FluentEntityMap<TEntity>> configure)
        {
            this.Entity(member, member.Name, configure);
        }

        /// <summary>
        /// Entities the specified member.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="member">The member.</param>
        /// <param name="documentKey">The document key.</param>
        /// <param name="configure">The configure.</param>
        public void Entity<TEntity>(MemberInfo member, string documentKey, Action<FluentEntityMap<TEntity>> configure)
        {
            var entityMap = new FluentEntityMap<TEntity>();
            configure(entityMap);
            this.Instance.AddEntityMap(new EntityMemberMap(member, entityMap.Instance));
        }

        /// <summary>
        /// Entities the specified member.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="member">The member.</param>
        /// <param name="configure">The configure.</param>
        public void Entity<TEntity>(Expression<Func<TDiscriminatedEntity, object>> member, Action<FluentEntityMap<TEntity>> configure)
        {
            if (member == null)
                throw new ArgumentNullException("member");

            var visitor = new MemberAccessMemberInfoVisitor();
            visitor.Visit(member);

            if (visitor.Members.Count() > 1)
                throw new NotSupportedException("Only top-level members are supported.");

            this.Entity(visitor.Members.Single(), configure);
        }

        /// <summary>
        /// Entities the specified member.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="member">The member.</param>
        /// <param name="documentKey">The document key.</param>
        /// <param name="configure">The configure.</param>
        public void Entity<TEntity>(Expression<Func<TDiscriminatedEntity, object>> member, string documentKey, Action<FluentEntityMap<TEntity>> configure)
        {
            if (member == null)
                throw new ArgumentNullException("member");

            var visitor = new MemberAccessMemberInfoVisitor();
            visitor.Visit(member);

            if (visitor.Members.Count() > 1)
                throw new NotSupportedException("Only top-level members are supported.");

            this.Entity(visitor.Members.Single(), documentKey, configure);
        }

        /// <summary>
        /// Maps the specified member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns></returns>
        public FluentMemberMap Map(MemberInfo member)
        {
            return this.Map(member, member.Name);
        }

        /// <summary>
        /// Maps the specified member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="documentKey">The document key.</param>
        /// <returns></returns>
        public FluentMemberMap Map(MemberInfo member, string documentKey)
        {
            var memberMap = new MemberMap(member, documentKey);
            this.Instance.AddMemberMap(memberMap);
            return new FluentMemberMap(memberMap);
        }

        /// <summary>
        /// Maps the specified member name.
        /// </summary>
        /// <param name="memberName">Name of the member.</param>
        public FluentMemberMap Map(string memberName)
        {
            return this.Map(memberName, memberName);
        }

        /// <summary>
        /// Maps the specified member name.
        /// </summary>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="documentKey">The document key.</param>
        public FluentMemberMap Map(string memberName, string documentKey)
        {
            var members = typeof(TDiscriminatedEntity).GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (members == null)
                throw new MissingMemberException(string.Format("{0}.{1} does not exist.", typeof(TDiscriminatedEntity), memberName));
            if (members.Length > 1)
                throw new NotSupportedException(string.Format("Unable to distinctly find member {0}.{1}", typeof(TDiscriminatedEntity), memberName));

            return this.Map(members[0], documentKey);
        }

        /// <summary>
        /// Maps the specified member.
        /// </summary>
        /// <param name="member">The member.</param>
        public FluentMemberMap Map(Expression<Func<TDiscriminatedEntity, object>> member)
        {
            if (member == null)
                throw new ArgumentNullException("member");

            var visitor = new MemberAccessMemberInfoVisitor();
            visitor.Visit(member);

            if (visitor.Members.Count() > 1)
                throw new NotSupportedException("Only top-level members are supported.");

            return this.Map(visitor.Members.Single());
        }

        /// <summary>
        /// Maps the specified member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="documentKey">The document key.</param>
        public FluentMemberMap Map(Expression<Func<TDiscriminatedEntity, object>> member, string documentKey)
        {
            if (member == null)
                throw new ArgumentNullException("member");
            if (documentKey == null)
                throw new ArgumentException("Cannot be null or empty.", "documentKey");

            var visitor = new MemberAccessMemberInfoVisitor();
            visitor.Visit(member);

            if (visitor.Members.Count() > 1)
                throw new NotSupportedException("Only top-level members are supported.");

            return this.Map(visitor.Members.Single(), documentKey);
        }

        #endregion
    }
}