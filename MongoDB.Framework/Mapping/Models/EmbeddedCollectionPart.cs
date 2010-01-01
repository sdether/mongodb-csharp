using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Mapping.Models
{
    public class EmbeddedCollectionPart
    {
        public ICollectionType CollectionType { get; set; }

        public Type ElementType { get; set; }

        public IValueType ElementValueType { get; set; }
    }
}