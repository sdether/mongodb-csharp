using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Types
{
    public class ListValueType : CollectionValueType
    {
        public override Type CollectionType
        {
            get { return typeof(List<>); }
        }
    }
}
