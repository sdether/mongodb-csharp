using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Linq.Visitors;

namespace MongoDB.Framework.Configuration
{
    public class EntityMap : DiscriminatedEntityMap
    {
        #region Private Fields

        private Dictionary<object, DiscriminatedEntityMap> discriminatedEntityMaps;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the discriminating document key.
        /// </summary>
        /// <value>The discriminating document key.</value>
        public string DiscriminatingDocumentKey { get; set; }

        /// <summary>
        /// Gets the discriminated entity maps.
        /// </summary>
        /// <value>The discriminated entity maps.</value>
        public IEnumerable<DiscriminatedEntityMap> DiscriminatedEntityMaps
        {
            get { return this.discriminatedEntityMaps.Values; }
        }

        /// <summary>
        /// Gets or sets the extended properties map.
        /// </summary>
        /// <value>The extended properties map.</value>
        public ExtendedPropertiesMap ExtendedPropertiesMap { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has discriminating document key.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has discriminating document key; otherwise, <c>false</c>.
        /// </value>
        public bool HasDiscriminatingValue
        {
            get { return this.DiscriminatingValue != null; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has extended properties.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has extended properties; otherwise, <c>false</c>.
        /// </value>
        public bool HasExtendedPropertiesMap
        {
            get { return this.ExtendedPropertiesMap != null; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is discriminated.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is discriminated; otherwise, <c>false</c>.
        /// </value>
        public bool IsDiscriminated
        {
            get { return this.discriminatedEntityMaps.Count > 0; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMap"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public EntityMap(Type type)
            : base(type)
        {
            this.discriminatedEntityMaps = new Dictionary<object, DiscriminatedEntityMap>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IMapVisitor visitor)
        {
            visitor.VisitEntityMap(this);
        }

        /// <summary>
        /// Adds the discriminated entity map.
        /// </summary>
        /// <param name="discriminatedEntityMap">The discriminated entity map.</param>
        public void AddDiscriminatedEntityMap(DiscriminatedEntityMap discriminatedEntityMap)
        {
            if (discriminatedEntityMap == null)
                throw new ArgumentNullException("discriminatedEntityMap");

            this.discriminatedEntityMaps[discriminatedEntityMap.DiscriminatingValue] = discriminatedEntityMap;
        }

        /// <summary>
        /// Gets the discriminated entity map by value.
        /// </summary>
        /// <param name="discriminatingValue">The discriminating value.</param>
        /// <returns></returns>
        public DiscriminatedEntityMap GetDiscriminatedEntityMapByValue(object discriminatingValue)
        {
            DiscriminatedEntityMap discriminatedEntityMap = null;
            if (!this.discriminatedEntityMaps.TryGetValue(discriminatingValue, out discriminatedEntityMap))                throw new UnknownDiscriminatorException(string.Format("The discriminate value {0} has not been mapped.", discriminatingValue));            
            
            return this.discriminatedEntityMaps[discriminatingValue];
        }

        /// <summary>
        /// Gets the discriminated entity map by type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public DiscriminatedEntityMap GetDiscriminatedEntityMapByType(Type type)
        {
            var discriminatedEntityMap = this.discriminatedEntityMaps.Values.SingleOrDefault(m => m.Type == type);            if (discriminatedEntityMap == null)                throw new UnmappedTypeException(string.Format("No discriminated entity mapped for type {0}", type));
             return discriminatedEntityMap;        }

        /// <summary>
        /// Gets the member map.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <returns></returns>
        public MemberMap GetMemberMap(Type entityType, string memberName)
        {
            if (this.Type == entityType)
                return this.GetMemberMap(memberName);

            var subMap = this.GetDiscriminatedEntityMapByType(entityType);
            return subMap.GetMemberMap(memberName);
        }

        #endregion
    }
}