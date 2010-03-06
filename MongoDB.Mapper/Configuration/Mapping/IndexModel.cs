using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Mapper.Mapping;

namespace MongoDB.Mapper.Configuration.Mapping
{
    public class IndexModel : ModelNode
    {
        public List<KeyValuePair<string, IndexDirection>> Parts { get; private set; }

        public string Name { get; set; }

        public bool IsUnique { get; set; }

        public IndexModel()
        {
            this.Parts = new List<KeyValuePair<string, IndexDirection>>();
        }
    }
}