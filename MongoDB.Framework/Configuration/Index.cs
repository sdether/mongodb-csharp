using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration
{
    public enum IndexDirection
    {
        Ascending,
        Descending
    }

    public class Index
    {
        public string Name { get; private set; }

        public IDictionary<string, IndexDirection> DocumentKeys { get; private set; }

        public bool IsUnique { get; set; }

        public Index(string name)
        {
            if (name == null)
                throw new ArgumentException("Cannot be null or empty.", "name");

            this.Name = name;
            this.DocumentKeys = new Dictionary<string, IndexDirection>();
        }
    }
}