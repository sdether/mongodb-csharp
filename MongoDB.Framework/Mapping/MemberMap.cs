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

        public Type MemberType { get; set; }

        public IValueType ValueType { get; set; }

        /// <summary>
        /// Maps the member from a document.
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        public virtual void MapFromDocument(MappingContext mappingContext)
        {
            if (mappingContext == null)
                throw new ArgumentNullException("mappingContext");
            var value = mappingContext.Document[this.Key];
            value = this.ValueType.ConvertFromDocumentValue(value, mappingContext);
            this.MemberSetter(mappingContext.Entity, value);
        }

        /// <summary>
        /// Maps the member to the document.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="document">The document.</param>
        public virtual void MapToDocument(object entity, Document document)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            if (document == null)
                throw new ArgumentNullException("document");
            var value = this.MemberGetter(entity);
            value = this.ValueType.ConvertToDocumentValue(value);
            document[this.Key] = value;
        }
    }
}