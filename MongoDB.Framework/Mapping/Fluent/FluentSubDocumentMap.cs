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
    public class FluentSubDocumentMap<TEntity> : FluentDocumentMap<SubDocumentMap, TEntity>
    {
        private SubDocumentMap instance;

        public override SubDocumentMap Instance
        {
            get { return this.instance; }
        }

        public FluentSubDocumentMap(RootDocumentMap rootDocumentMap)
        {
            this.instance = new SubDocumentMap(typeof(TEntity), rootDocumentMap);
        }
    }
}