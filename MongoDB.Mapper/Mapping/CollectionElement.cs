using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper.Mapping
{
    public class CollectionElement
    {
        public object Element { get; set; }

        public object CustomData { get; set; }
    }
}