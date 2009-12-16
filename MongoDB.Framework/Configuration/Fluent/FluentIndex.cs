using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Fluent
{
    public class FluentIndex
    {
        private Index instance;

        public FluentIndex(string name)
            : this(new Index(name))
        { }

        public FluentIndex(Index index)
        {
            if (index == null)
                throw new ArgumentNullException("index");

            this.instance = index;
        }

        public FluentIndex Unique()
        {
            this.instance.IsUnique = true;
            return this;
        }

        public FluentIndex Ascending(string key)
        {
            return this.Key(key, IndexDirection.Ascending);
        }

        public FluentIndex Descending(string key)
        {
            return this.Key(key, IndexDirection.Descending);
        }

        public FluentIndex Key(string key, IndexDirection direction)
        {
            this.instance.DocumentKeys[key] = direction;
            return this;
        }
    }
}