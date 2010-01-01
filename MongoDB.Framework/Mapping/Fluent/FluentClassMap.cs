using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Linq.Visitors;
using MongoDB.Framework.Reflection;
using MongoDB.Framework.Mapping.Models;
using MongoDB.Framework.Mapping.Types;

namespace MongoDB.Framework.Mapping.Fluent
{
    public abstract class FluentClassMap<TModel, TEntity> : FluentBase<TModel> where TModel : ClassMapModel
    {
        public FluentMap<TModel, TEntity> Map
        {
            get { return new FluentMap<TModel, TEntity>(this); }
        }

        public FluentClassMap(TModel model)
            : base(model)
        { }
    }
}