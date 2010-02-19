using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    public class FluentMember : FluentPersistentMember<PersistentMemberMapModel, FluentMember>
    {
        private ClassMapModel classMapModel;

        protected override FluentMember Fluent
        {
            get { return this; }
        }

        public FluentMember(ClassMapModel classMapModel)
            : base(new PersistentMemberMapModel())
        {
            this.classMapModel = classMapModel;
        }

        public FluentCollection AsCollection()
        {
            var value = new FluentCollection();
            value.Model.Getter = this.Model.Getter;
            value.Model.Setter = this.Model.Setter;
            value.Model.Key = this.Model.Key;
            value.Model.PersistNull = this.Model.PersistNull;

            classMapModel.PersistentMemberMaps.Remove(this.Model);
            classMapModel.PersistentMemberMaps.Add(value.Model);
            return value;
        }

        public FluentReference AsReference()
        {
            var value = new FluentReference();
            value.Model.Getter = this.Model.Getter;
            value.Model.Setter = this.Model.Setter;
            value.Model.Key = this.Model.Key;
            value.Model.PersistNull = this.Model.PersistNull;

            classMapModel.PersistentMemberMaps.Remove(this.Model);
            classMapModel.PersistentMemberMaps.Add(value.Model);
            return value;
        }

        public FluentConvertibleMember ConvertWith<TConverter>() where TConverter : IValueConverter, new()
        {
            return this.ConvertWith(new TConverter());
        }

        public FluentConvertibleMember ConvertWith(IValueConverter valueConverter)
        {
            var value = new FluentConvertibleMember();
            value.Model.Getter = this.Model.Getter;
            value.Model.Setter = this.Model.Setter;
            value.Model.Key = this.Model.Key;
            value.Model.PersistNull = this.Model.PersistNull;
            value.Model.ValueConverter = valueConverter;

            classMapModel.PersistentMemberMaps.Remove(this.Model);
            classMapModel.PersistentMemberMaps.Add(value.Model);
            return value;
        }
    }
}