using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping.Models
{
    public class IndexModel : MapModel
    {
        public List<KeyValuePair<string, IndexDirection>> Parts { get; private set; }

        public string Name { get; set; }

        public bool IsUnique { get; set; }

        public IndexModel()
        {
            this.Parts = new List<KeyValuePair<string, IndexDirection>>();
        }

        public override void Accept(IMapModelVisitor visitor)
        {
            visitor.ProcessIndex(this);
        }
    }
}