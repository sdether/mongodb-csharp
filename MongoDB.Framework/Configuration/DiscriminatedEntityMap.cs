using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Visitors;

namespace MongoDB.Framework.Configuration
{
    public class DiscriminatedEntityMap
    {
        #region Private Fields

        private Dictionary<string, MemberMap> memberMaps;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the discriminating value.
        /// </summary>
        /// <value>The discriminating value.</value>
        public object DiscriminatingValue { get; set; }

        /// <summary>
        /// Gets the member maps.
        /// </summary>
        /// <value>The member maps.</value>
        public IEnumerable<MemberMap> MemberMaps
        {
            get { return this.memberMaps.Values; }
        }

        /// <summary>
        /// Gets the entity's type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscriminatedEntityMap"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public DiscriminatedEntityMap(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            this.memberMaps = new Dictionary<string, MemberMap>();
            this.Type = type;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the entity map.
        /// </summary>
        /// <param name="entityMap">The entity map.</param>
        public void AddEntityMap(EntityMemberMap entityMap)
        {
            if (entityMap == null)
                throw new ArgumentNullException("entityMap");

            this.memberMaps[entityMap.DocumentKey] = entityMap;
        }

        /// <summary>
        /// Adds the member map.
        /// </summary>
        /// <param name="memberMap">The member map.</param>
        public void AddMemberMap(MemberMap memberMap)
        {
            if (memberMap == null)
                throw new ArgumentNullException("memberMap");

            this.memberMaps[memberMap.DocumentKey] = memberMap;
        }

        /// <summary>
        /// Gets the member map.
        /// </summary>
        /// <param name="memberName">Name of the member.</param>
        /// <returns></returns>
        public MemberMap GetMemberMap(string memberName)
        {
            var memberMap = this.memberMaps.Values.FirstOrDefault(m => m.MemberName == memberName);
            if (memberMap == null)
                throw new UnmappedMemberException(string.Format("{0}.{1} has not been mapped.", this.Type, memberName));

            return memberMap;
        }

        #endregion

        #region Private Methods

        private bool IsDictionaryOfStringObject(Type type)
        {
            return true;
        }

        #endregion
    }
}