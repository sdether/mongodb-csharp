using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Linq.Visitors;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    public class FluentRootClass<TRootClass> : FluentSuperClass<RootClassMapModel, TRootClass>
    {
        public string CollectionName
        {
            get { return this.Model.CollectionName; }
            set { this.Model.CollectionName = value; }
        }

        public FluentRootClass()
            : base(new RootClassMapModel(typeof(TRootClass)))
        { }

        public FluentIndex<TRootClass> HasIndex()
        {
            var fluentIndex = new FluentIndex<TRootClass>();
            this.Model.Indexes.Add(fluentIndex.Model);
            return fluentIndex;
        }
    }
}