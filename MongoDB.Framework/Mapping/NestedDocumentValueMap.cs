using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class NestedDocumentValueMap : ValueMap
    {
        public DocumentMap DocumentMap { get; private set; }

        public NestedDocumentValueMap(MappingStore metaDataStore, string key, string memberName, Type memberType, Func<object, object> memberGetter, Action<object, object> memberSetter, DocumentMap document)
            : base(metaDataStore, key, memberName, memberType, memberGetter, memberSetter)
        {
            if (document == null)
                throw new ArgumentNullException("document");

            this.DocumentMap = document;
        }
    }
}