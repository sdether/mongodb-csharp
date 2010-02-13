using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using MongoDB.Driver;
using MongoDB.Framework.Persistence;
using MongoDB.Framework.Mapping.Visitors;

namespace MongoDB.Framework.Mapping
{
    public class MemberMapPath
    {
        #region Private Fields

        private Type type;
        private IMappingStore mappingStore;
        private IEnumerable<string> memberNames;

        private List<PersistentMemberMap> memberMaps;

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
        public object ConvertToDocumentValue(object value, IMongoSessionImplementor mongoSession)
        {
            var lastMemberMap = this.memberMaps[this.memberMaps.Count - 1];
            var mapper = new ValueToDocumentValueMapper(mongoSession);
            return mapper.CreateDocumentValue(lastMemberMap, value);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="memberNames">The member names.</param>
        private void Initialize(IEnumerable<string> memberNames)
        {
            this.memberMaps = new List<PersistentMemberMap>();
            var classMap = this.mappingStore.GetClassMapFor(this.type);

            var memberNamesEnumerator = memberNames.GetEnumerator();
            memberNamesEnumerator.MoveNext();
            var currentMemberMap = classMap.GetMemberMapBaseFromMemberName(memberNamesEnumerator.Current) as PersistentMemberMap;
            if (currentMemberMap == null)
                throw new InvalidOperationException("Only persistent member maps are allowed.");
            this.Key = currentMemberMap.Key;
            this.memberMaps.Add(currentMemberMap);

            while (memberNamesEnumerator.MoveNext())
            {
                currentMemberMap = this.GetNextMemberMap(currentMemberMap, memberNamesEnumerator.Current);
                this.memberMaps.Add(currentMemberMap);
            }
        }


        /// <summary>
        /// Gets the next member map.
        /// </summary>
        /// <param name="currentMemberMap">The current member map.</param>
        /// <param name="nextMemberName">Name of the next member.</param>
        /// <returns></returns>
        private PersistentMemberMap GetNextMemberMap(PersistentMemberMap currentMemberMap, string nextMemberName)
        {
            var memberMap = currentMemberMap as ValueTypeMemberMap;
            if (memberMap != null && memberMap.ValueType is NestedClassValueType)
            {
                var nestedClassMap = ((NestedClassValueType)memberMap.ValueType).NestedClassMap;
                currentMemberMap = nestedClassMap.GetMemberMapBaseFromMemberName(nextMemberName) as PersistentMemberMap;
                if (currentMemberMap == null)
                    throw new InvalidOperationException("Only persistent member maps are allowed.");
                this.Key += currentMemberMap.Key;
                return currentMemberMap;
            }
            else if (memberMap != null && memberMap.ValueType is ManyToOneValueType)
            {
                var refType = ((ManyToOneValueType)memberMap.ValueType).ReferenceType;
                var classMap = this.mappingStore.GetClassMapFor(refType);
                if (classMap.IdMap.MemberName == nextMemberName)
                {
                    this.Key += ".$id";
                    return classMap.IdMap;
                }
                else
                    throw new NotSupportedException("Id is the only supported condition across a reference.");
            }

            throw new NotSupportedException(string.Format("The value type {0} must occur last.", currentMemberMap));
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
