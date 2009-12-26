using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping
{
    public class MemberMap : Map
    {
        public string Key { get; set; }

        public string MemberName { get; set; }

        public Func<object, object> MemberGetter { get; set; }

        public Action<object, object> MemberSetter { get; set; }

        public IValueType ValueType { get; set; }

        /// <summary>
        /// Translates from document.
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        public virtual void TranslateFromDocument(MappingContext mappingContext)
        {
            var value = mappingContext.Document[this.Key];
            value = this.ValueType.ConvertFromDocumentValue(value, mappingContext);
            this.MemberSetter(mappingContext.Entity, value);
        }

        /// <summary>
        /// Translates to document.
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        public virtual void TranslateToDocument(MappingContext mappingContext)
        {
            var value = this.MemberGetter(mappingContext.Entity);
            value = this.ValueType.ConvertToDocumentValue(value, mappingContext);
            mappingContext.Document[this.Key] = value;
        }
    }
}