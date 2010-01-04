using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Configuration.Mapping.Models;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    public class FluentIndex<TEntity> : FluentBase<IndexModel>
    {
        public FluentIndex()
            : base(new IndexModel())
        { }

        /// <summary>
        /// Adds the ascending.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public FluentIndex<TEntity> Ascending(string key)
        {
            this.Model.Parts.Add(new KeyValuePair<string, IndexDirection>(key, IndexDirection.Ascending));
            return this;
        }

        /// <summary>
        /// Adds the descending.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public FluentIndex<TEntity> Descending(string key)
        {
            this.Model.Parts.Add(new KeyValuePair<string, IndexDirection>(key, IndexDirection.Descending));
            return this;
        }

        /// <summary>
        /// Names the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public FluentIndex<TEntity> Name(string name)
        {
            this.Model.Name = name;
            return this;
        }

        /// <summary>
        /// Uniques this instance.
        /// </summary>
        /// <returns></returns>
        public FluentIndex<TEntity> Unique()
        {
            this.Model.IsUnique = true;
            return this;
        }
    }
}