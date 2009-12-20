using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Linq.Visitors;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Configuration.Fluent
{
    public class FluentRootEntityMap<TRootEntity> : FluentEntityMap<TRootEntity>
    {
        #region Public Properties

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public new RootEntityMap Instance
        {
            get { return (RootEntityMap)base.Instance; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMap&lt;T&gt;"/> class.
        /// </summary>
        public FluentRootEntityMap()
            : base(new RootEntityMap(typeof(TRootEntity)))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMap&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="rootEntityMap">The root entity map.</param>
        public FluentRootEntityMap(RootEntityMap rootEntityMap)
            : base(rootEntityMap)
        { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Maps the specified member.
        /// </summary>
        /// <param name="member">The member.</param>
        public void Id(MemberInfo member)
        {
            var getter = LateBoundReflection.GetGetter(member);
            var setter = LateBoundReflection.GetSetter(member);
            this.Instance.IdMap = new IdMap(member.Name, getter, setter);
        }

        /// <summary>
        /// Maps the specified member name.
        /// </summary>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="documentKey">The document key.</param>
        public void Id(string memberName)
        {
            var members = typeof(TRootEntity).GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (members == null)
                throw new MissingMemberException(string.Format("{0}.{1} does not exist.", typeof(TRootEntity), memberName));
            if (members.Length > 1)
                throw new NotSupportedException(string.Format("Unable to distinctly find member {0}.{1}", typeof(TRootEntity), memberName));

            this.Id(members[0]);
        }

        /// <summary>
        /// Maps the specified member.
        /// </summary>
        /// <param name="member">The member.</param>
        public void Id(Expression<Func<TRootEntity, string>> member)
        {
            if (member == null)
                throw new ArgumentNullException("member");

            var members = MemberInfoPathExtractor.ExtractFrom(member);

            if (members.Count > 1)
                throw new NotSupportedException("Only top-level members are supported.");

            this.Id(members[0]);
        }

        /// <summary>
        /// Indexes the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public FluentIndex Index(string name)
        {
            var index = new Index(name);
            this.Instance.AddIndex(index);
            return new FluentIndex(index);
        }

        /// <summary>
        /// Withes the name of the collection.
        /// </summary>
        /// <param name="name">The name.</param>
        public void WithCollectionName(string name)
        {
            if (name == null)
                throw new ArgumentException("Cannot be null or empty.", "name");


            this.Instance.CollectionName = name;
        }

        #endregion
    }
}