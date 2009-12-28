using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Mapping.Model;
using MongoDB.Framework.Mapping.Types;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Mapping.Fluent
{
    public abstract class FluentMemberMap<TModel> : FluentMap<TModel> where TModel : MemberMapModel
    {
        public FluentMemberMap(TModel model)
            : base(model)
        { }
    }
}