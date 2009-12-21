using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class NestedDocumentValueMap : ValueMap
    {
        public RootDocumentMap RootDocumentMap { get; private set; }

        public NestedDocumentValueMap(string key, string memberName, Type memberType, Func<object, object> memberGetter, Action<object, object> memberSetter, RootDocumentMap rootDocumentMap)
            : base(key, memberName, memberType, memberGetter, memberSetter)
        {
            if (rootDocumentMap == null)
                throw new ArgumentNullException("rootDocumentMap");

            this.RootDocumentMap = rootDocumentMap;
        }
    }
}