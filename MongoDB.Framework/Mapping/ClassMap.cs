using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Framework.Configuration.Mapping;

namespace MongoDB.Framework.Mapping
{
    public abstract class ClassMap : MapNode
    {
        #region Private Fields

        private readonly List<MemberMap> memberMaps;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        /// <value>The name of the collection.</value>
        public abstract string CollectionName { get; internal set; }

        /// <summary>
        /// Gets or sets the discriminator.
        /// </summary>
        /// <value>The discriminator.</value>
        public object Discriminator { get; internal set; }

        /// <summary>
        /// Gets or sets the discriminator key.
        /// </summary>
        /// <value>The discriminator key.</value>
        public abstract string DiscriminatorKey { get; internal set; }

        /// <summary>
        /// Gets the extended properties map.
        /// </summary>
        /// <value>The extended properties map.</value>
        public abstract ExtendedPropertiesMap ExtendedPropertiesMap { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this instance has extended properties.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has extended properties; otherwise, <c>false</c>.
        /// </value>
        public abstract bool HasExtendedProperties { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has id.
        /// </summary>
        /// <value><c>true</c> if this instance has id; otherwise, <c>false</c>.</value>
        public abstract bool HasId { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has indexes.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has indexes; otherwise, <c>false</c>.
        /// </value>
        public abstract bool HasIndexes { get; }

        /// <summary>
        /// Gets the id map.
        /// </summary>
        /// <value>The id map.</value>
        public abstract IdMap IdMap { get; internal set; }

        /// <summary>
        /// Gets the indexes.
        /// </summary>
        /// <value>The indexes.</value>
        public abstract IEnumerable<Index> Indexes { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is polymorphic.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is polymorphic; otherwise, <c>false</c>.
        /// </value>
        public abstract bool IsPolymorphic { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is root.
        /// </summary>
        /// <value><c>true</c> if this instance is root; otherwise, <c>false</c>.</value>
        public abstract bool IsRoot { get; }

        /// <summary>
        /// Gets the member maps.
        /// </summary>
        /// <value>The simple member maps.</value>
        public virtual IEnumerable<MemberMap> MemberMaps
        {
            get { return this.memberMaps; }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type { get; private set; }

        public virtual IClassActivator ClassActivator { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassMap"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="memberMaps">The member maps.</param>
        /// <param name="discriminator">The discriminator.</param>
        protected ClassMap(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            this.memberMaps = new List<MemberMap>();
            this.Type = type;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IMapVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        /// Gets the class map by discriminator.
        /// </summary>
        /// <param name="discriminator">The discriminator.</param>
        /// <returns></returns>
        public abstract ClassMap GetClassMapByDiscriminator(object discriminator);

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public object GetId(object entity)
        {
            if(!this.HasId)
                throw new InvalidOperationException("Entity doesn't have an id.");

            return this.IdMap.MemberGetter(entity);
        }

        /// <summary>
        /// Gets the name of the member map from member.
        /// </summary>
        /// <param name="memberName">Name of the member.</param>
        /// <returns></returns>
        public MemberMap GetMemberMapBaseFromMemberName(string memberName)
        {
            if (this.HasId && this.IdMap.MemberName == memberName)
                return this.IdMap;
            MemberMap memberMap = this.MemberMaps.FirstOrDefault(x => x.MemberName == memberName);
            if (memberMap != null)
                return memberMap;

            throw new UnmappedMemberException(string.Format("The member {0} has not been mapped.", memberName));
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Adds the member map.
        /// </summary>
        /// <param name="memberMap">The member map.</param>
        internal void AddMemberMap(MemberMap memberMap)
        {
            if (memberMap == null)
                throw new ArgumentNullException("memberMap");

            this.memberMaps.Add(memberMap);
        }

        /// <summary>
        /// Adds the member maps.
        /// </summary>
        /// <param name="memberMaps">The member maps.</param>
        internal void AddMemberMaps(IEnumerable<MemberMap> memberMaps)
        {
            if (memberMaps == null)
                throw new ArgumentNullException("memberMaps");

            this.memberMaps.AddRange(memberMaps);
        }

        #endregion
    }
}