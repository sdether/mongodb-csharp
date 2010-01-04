using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MongoDB.Framework.Persistence;
using MongoDB.Driver;
using MongoDB.Framework.Configuration.Mapping.Types;

namespace MongoDB.Framework.Configuration.Mapping
{
    public class MemberMapPath
    {
        #region Private Fields

        private Type type;
        private IMappingStore mappingStore;
        private IEnumerable<string> memberNames;

        private List<MemberMap> memberMaps;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberMapPath"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="type">ValueType of the entity.</param>
        /// <param name="memberNames">The member names.</param>
        public MemberMapPath(IMappingStore mappingStore, Type type, params string[] memberNames)
            : this(mappingStore, type, (IEnumerable<string>)memberNames)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberMapPath"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="type">ValueType of the entity.</param>
        /// <param name="memberNames">The member names.</param>
        public MemberMapPath(IMappingStore mappingStore, Type type, IEnumerable<string> memberNames)
        {
            if (mappingStore == null)
                throw new ArgumentNullException("mappingStore");
            if (type == null)
                throw new ArgumentNullException("type");
            if (memberNames == null)
                throw new ArgumentNullException("memberNames");
            this.type = type;
            this.mappingStore = mappingStore;

            this.Initialize(memberNames);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberMapPath"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="type">ValueType of the entity.</param>
        /// <param name="memberInfos">The member infos.</param>
        public MemberMapPath(IMappingStore mappingStore, Type type, params MemberInfo[] memberInfos)
            : this(mappingStore, type, (IEnumerable<MemberInfo>)memberInfos)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberMapPath"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="type">ValueType of the entity.</param>
        /// <param name="memberInfos">The member infos.</param>
        public MemberMapPath(IMappingStore mappingStore, Type type, IEnumerable<MemberInfo> memberInfos)
            : this(mappingStore, type, memberInfos.Select(mi => mi.Name))
        { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts to document value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public object ConvertToDocumentValue(object value, IMongoContext mongoContext)
        {
            var lastMemberMap = this.memberMaps[this.memberMaps.Count - 1];
            return lastMemberMap.ValueType.ConvertToDocumentValue(value, mongoContext);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="memberNames">The member names.</param>
        private void Initialize(IEnumerable<string> memberNames)
        {
            this.memberMaps = new List<MemberMap>();
            var classMap = this.mappingStore.GetClassMapFor(this.type);

            var memberNamesEnumerator = memberNames.GetEnumerator();
            memberNamesEnumerator.MoveNext();
            var currentMemberMap = classMap.GetMemberMapFromMemberName(memberNamesEnumerator.Current);
            this.Key = currentMemberMap.Key;
            this.memberMaps.Add(currentMemberMap);

            while (memberNamesEnumerator.MoveNext())
            {
                currentMemberMap = this.GetNextMemberMap(currentMemberMap, memberNamesEnumerator.Current);
                this.Key += "." + currentMemberMap.Key;
                this.memberMaps.Add(currentMemberMap);
            }
        }


        /// <summary>
        /// Gets the next member map.
        /// </summary>
        /// <param name="currentMemberMap">The current member map.</param>
        /// <param name="nextMemberName">Name of the next member.</param>
        /// <returns></returns>
        private MemberMap GetNextMemberMap(MemberMap currentMemberMap, string nextMemberName)
        {
            if (currentMemberMap.ValueType is NestedClassValueType)
            {
                var nestedClassMAp = ((NestedClassValueType)currentMemberMap.ValueType).NestedClassMap;
                return nestedClassMAp.GetMemberMapFromMemberName(nextMemberName);
            }

            throw new NotSupportedException(string.Format("The value type {0} must occur last.", currentMemberMap.ValueType.GetType()));
        }

        #endregion
    }

    public class MemberMapPath<TEntity> : MemberMapPath
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberMapPath&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="memberNames">The member names.</param>
        public MemberMapPath(IMappingStore mappingStore, params string[] memberNames)
            : base(mappingStore, typeof(TEntity), memberNames)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberMapPath&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="memberNames">The member names.</param>
        public MemberMapPath(IMappingStore mappingStore, IEnumerable<string> memberNames)
            : base(mappingStore, typeof(TEntity), memberNames)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberMapPath&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="memberInfos">The member infos.</param>
        public MemberMapPath(IMappingStore mappingStore, params MemberInfo[] memberInfos)
            : base(mappingStore, typeof(TEntity), memberInfos)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberMapPath&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="memberInfos">The member infos.</param>
        public MemberMapPath(IMappingStore mappingStore, IEnumerable<MemberInfo> memberInfos)
            : base(mappingStore, typeof(TEntity), memberInfos)
        { }

        #endregion
    }
}
