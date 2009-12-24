using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Linq.Visitors;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentSubClassMap<TEntity> : FluentClassMap<SubClassMap, TEntity>
    {
        private SubClassMap instance;

        public override SubClassMap Instance
        {
            get { return this.instance; }
        }

        public FluentSubClassMap(SuperClassMap rootClassMap)
        {
            this.instance = new SubClassMap(typeof(TEntity), rootClassMap);
        }
    }
}