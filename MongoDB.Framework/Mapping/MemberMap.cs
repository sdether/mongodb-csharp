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
        /// <param name="translationContext">The translation context.</param>
        public virtual void TranslateFromDocument(TranslationContext translationContext)
        {
            var value = translationContext.Document[this.Key];
            value = this.ValueType.ConvertFromDocumentValue(value, translationContext);
            this.MemberSetter(translationContext.Owner, value);
        }

        /// <summary>
        /// Translates to document.
        /// </summary>
        /// <param name="translationContext">The translation context.</param>
        public virtual void TranslateToDocument(TranslationContext translationContext)
        {
            var value = this.MemberGetter(translationContext.Owner);
            value = this.ValueType.ConvertToDocumentValue(value, translationContext);
            translationContext.Document[this.Key] = value;
        }
    }
}