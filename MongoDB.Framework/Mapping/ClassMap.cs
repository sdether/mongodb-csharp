using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping
{
    public abstract class ClassMap : Map
    {
        #region Private Fields

        private readonly Dictionary<string, MemberMap> memberMaps;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        /// <value>The name of the collection.</value>
        public abstract string CollectionName { get; set; }

        /// <summary>
        /// Gets or sets the discriminator.
        /// </summary>
        /// <value>The discriminator.</value>
        public object Discriminator { get; set; }

        /// <summary>
        /// Gets or sets the discriminator key.
        /// </summary>
        /// <value>The discriminator key.</value>
        public abstract string DiscriminatorKey { get; set; }

        /// <summary>
        /// Gets the extended properties map.
        /// </summary>
        /// <value>The extended properties map.</value>
        public abstract ExtendedPropertiesMap ExtendedPropertiesMap { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has extended properties.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has extended properties; otherwise, <c>false</c>.
        /// </value>
        public bool HasExtendedProperties
        {
            get { return this.ExtendedPropertiesMap != null; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has id.
        /// </summary>
        /// <value><c>true</c> if this instance has id; otherwise, <c>false</c>.</value>
        public bool HasId
        {
            get { return this.IdMap != null; }
        }

        /// <summary>
        /// Gets the id map.
        /// </summary>
        /// <value>The id map.</value>
        public virtual IdMap IdMap
        {
            get { return null; }
            set { throw new NotSupportedException("IdMap can only be set on a CollectionMap."); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is polymorphic.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is polymorphic; otherwise, <c>false</c>.
        /// </value>
        public abstract bool IsPolymorphic { get; }

        /// <summary>
        /// Gets the member maps.
        /// </summary>
        /// <value>The simple member maps.</value>
        public virtual IEnumerable<MemberMap> MemberMaps
        {
            get
            {
                foreach (var memberMap in this.memberMaps.Values)
                    yield return memberMap;
            }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassMap"/> class.
        /// </summary>
        /// <param name="metaDataStore">The meta data store.</param>
        /// <param name="type">ValueType of the entity.</param>
        public ClassMap(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            this.memberMaps = new Dictionary<string, MemberMap>();
            this.Type = type;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the member map.
        /// </summary>
        /// <param name="memberMap">The member map.</param>
        public void AddMemberMap(MemberMap memberMap)
        {
            if (memberMap == null)
                throw new ArgumentNullException("value");

            if (this.ContainsKey(memberMap.Key))
                throw new InvalidOperationException(string.Format("An item with key {0} has already been added.", memberMap.Key));

            this.memberMaps.Add(memberMap.Key, memberMap);
        }

        /// <summary>
        /// Gets the class map by discriminator.
        /// </summary>
        /// <param name="discriminator">The discriminator.</param>
        /// <returns></returns>
        public abstract ClassMap GetClassMapByDiscriminator(object discriminator);

        /// <summary>
        /// Gets the name of the member map from member.
        /// </summary>
        /// <param name="memberName">Name of the member.</param>
        /// <returns></returns>
        public MemberMap GetMemberMapFromMemberName(string memberName)
        {
            if (this.HasId && this.IdMap.MemberName == memberName)
                return this.IdMap;
            MemberMap memberMap = this.MemberMaps.FirstOrDefault(x => x.MemberName == memberName);
            if (memberMap != null)
                return memberMap;

            throw new UnmappedMemberException(string.Format("The member {0} has not been mapped.", memberName));
        }

        /// <summary>
        /// Translates from document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public virtual object TranslateFromDocument(Document document)
        {
            var translationContext = new TranslationContext() { Document = document };
            this.TranslateFromDocument(translationContext);
            return translationContext.Owner;
        }

        /// <summary>
        /// Translates from document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="owner">The owner.</param>
        /// <returns></returns>
        public virtual void TranslateFromDocument(TranslationContext translationContext)
        {
            if (translationContext.Owner == null)
            {
                var concreteType = this.GetConcreteType(translationContext.Document);
                translationContext.CreateOwner(concreteType);
            }

            if (this.HasId)
            {
                this.IdMap.TranslateFromDocument(translationContext);
                translationContext.Document.Remove(this.IdMap.Key);
            }

            foreach (var memberMap in this.MemberMaps)
            {
                memberMap.TranslateFromDocument(translationContext);
                translationContext.Document.Remove(memberMap.Key);
            }

            if (this.IsPolymorphic)
                translationContext.Document.Remove(this.DiscriminatorKey);

            if (this.HasExtendedProperties)
            {
                var dictionary = translationContext.Document.ToDictionary();
                this.ExtendedPropertiesMap.MemberSetter(translationContext.Owner, dictionary);
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Creates the owner.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        protected abstract Type GetConcreteType(Document document);

        #endregion

        #region Private Methods

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// 	<c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        private bool ContainsKey(string key)
        {
            return (key == "_id" && this.HasId)
                || this.memberMaps.ContainsKey(key);
        }

        #endregion
    }
}