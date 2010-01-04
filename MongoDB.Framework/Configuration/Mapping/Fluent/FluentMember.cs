using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Configuration.Mapping.Models;

namespace MongoDB.Framework.Configuration.Mapping.Fluent
{
    public abstract class FluentMember<TModel, TFluent> : FluentBase<TModel> where TModel : MemberMapModelBase
    {
        protected abstract TFluent Fluent { get; }

        public FluentMember(TModel model)
            : base(model)
        { }

        public TFluent Key(string key)
        {
            this.Model.Key = key;
            return this.Fluent;
        }
    }
}