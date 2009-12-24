using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MongoDB.Framework.Persistence;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping
{
    public class ValueMapPath
    {
        #region Private Fields

        private Type type;
        private MappingStore mappingStore;
        private IEnumerable<string> memberNames;

        private List<ValueMap> valueMaps;

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
        /// Initializes a new instance of the <see cref="ValueMapPath"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="type">Type of the entity.</param>
        /// <param name="memberNames">The member names.</param>
        public ValueMapPath(MappingStore mappingStore, Type type, params string[] memberNames)
            : this(mappingStore, type, (IEnumerable<string>)memberNames)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueMapPath"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="type">Type of the entity.</param>
        /// <param name="memberNames">The member names.</param>
        public ValueMapPath(MappingStore mappingStore, Type type, IEnumerable<string> memberNames)
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
        /// Initializes a new instance of the <see cref="ValueMapPath"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="type">Type of the entity.</param>
        /// <param name="memberInfos">The member infos.</param>
        public ValueMapPath(MappingStore mappingStore, Type type, params MemberInfo[] memberInfos)
            : this(mappingStore, type, (IEnumerable<MemberInfo>)memberInfos)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueMapPath"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="type">Type of the entity.</param>
        /// <param name="memberInfos">The member infos.</param>
        public ValueMapPath(MappingStore mappingStore, Type type, IEnumerable<MemberInfo> memberInfos)
            : this(mappingStore, type, memberInfos.Select(mi => mi.Name))
        { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts to document value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public object ConvertToDocumentValue(object entityValue)
        {
            var lastValueMap = this.valueMaps[this.valueMaps.Count - 1];

            if (lastValueMap is SimpleValueMap)
                return MongoTypeConverter.ConvertToDocumentValue(((SimpleValueMap)lastValueMap).MemberType, entityValue);
            else if (lastValueMap is IdMap)
                return MongoTypeConverter.ConvertToOid((string)entityValue);
            else if (lastValueMap is ComponentValueMap)
            {
                var componentValueMap = (ComponentValueMap)lastValueMap;
                return new EntityToDocumentTranslator(this.mappingStore)
                    .Translate(componentValueMap.ComponentClassMap, entityValue);
            }

            throw new NotSupportedException();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="memberNames">The member names.</param>
        private void Initialize(IEnumerable<string> memberNames)
        {
            this.valueMaps = new List<ValueMap>();
            var classMap = this.mappingStore.GetClassMapFor(this.type);

            var memberNamesEnumerator = memberNames.GetEnumerator();
            memberNamesEnumerator.MoveNext();
            var currentValueMap = classMap.GetValueMapFromMemberName(memberNamesEnumerator.Current);
            this.Key = currentValueMap.Key;
            this.valueMaps.Add(currentValueMap);

            while (memberNamesEnumerator.MoveNext())
            {
                currentValueMap = this.GetNextValueMap(currentValueMap, memberNamesEnumerator.Current);
                this.Key += "." + currentValueMap.Key;
                this.valueMaps.Add(currentValueMap);
            }
        }


        /// <summary>
        /// Gets the next value map.
        /// </summary>
        /// <param name="currentValueMap">The current value map.</param>
        /// <param name="nextMemberName">Name of the next member.</param>
        /// <returns></returns>
        private ValueMap GetNextValueMap(ValueMap currentValueMap, string nextMemberName)
        {
            if (currentValueMap is ComponentValueMap)
            {
                var componentValueMap = (ComponentValueMap)currentValueMap;
                return componentValueMap.ComponentClassMap.GetValueMapFromMemberName(nextMemberName);
            }
            else if (currentValueMap is ReferenceValueMap)
            {
                throw new InvalidOperationException("Cannot create a ValueMapPath using a ReferenceValueMap.");
            }
            else if (currentValueMap is SimpleValueMap)
            {
                throw new InvalidOperationException("SimpleValueMaps can only occur at the end of a path.");
            }
            else if (currentValueMap is IdMap)
            {
                throw new InvalidOperationException("SimpleValueMaps can only occur at the end of a path.");
            }

            throw new NotSupportedException(string.Format("Unknown ValueMap type {0}.", currentValueMap.GetType()));
        }

        #endregion
    }

    public class ValueMapPath<TEntity> : ValueMapPath
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueMapPath&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="memberNames">The member names.</param>
        public ValueMapPath(MappingStore mappingStore, params string[] memberNames)
            : base(mappingStore, typeof(TEntity), memberNames)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueMapPath&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="memberNames">The member names.</param>
        public ValueMapPath(MappingStore mappingStore, IEnumerable<string> memberNames)
            : base(mappingStore, typeof(TEntity), memberNames)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueMapPath&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="memberInfos">The member infos.</param>
        public ValueMapPath(MappingStore mappingStore, params MemberInfo[] memberInfos)
            : base(mappingStore, typeof(TEntity), memberInfos)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueMapPath&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="memberInfos">The member infos.</param>
        public ValueMapPath(MappingStore mappingStore, IEnumerable<MemberInfo> memberInfos)
            : base(mappingStore, typeof(TEntity), memberInfos)
        { }

        #endregion
    }
}
