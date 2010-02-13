using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Configuration.Mapping;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    public abstract class FluentMember<TModel, TFluent> : FluentBase<TModel> where TModel : PersistentMemberMapModel
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

        public TFluent PersistNull(bool value)
        {
            this.Model.PersistNull = value;
            return this.Fluent;
        }
    }
}