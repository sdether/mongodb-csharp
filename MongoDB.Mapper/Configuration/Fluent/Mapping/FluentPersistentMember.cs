using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Mapper.Configuration.Mapping;

namespace MongoDB.Mapper.Configuration.Fluent.Mapping
{
    public abstract class FluentPersistentMember<TModel, TFluent> : FluentBase<TModel> where TModel : PersistentMemberMapModel
    {
        protected abstract TFluent Fluent { get; }

        public FluentPersistentMember(TModel model)
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