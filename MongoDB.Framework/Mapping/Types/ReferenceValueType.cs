using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;

namespace MongoDB.Framework.Mapping.Types
{
    public class ReferenceValueType : NullSafeValueType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceValueType"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public ReferenceValueType(Type type)
            : base(type)
        { }

        /// <summary>
        /// Converts from document value.
        /// </summary>
        /// <param name="documentValue">The document value.</param>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns></returns>
        public override object ConvertFromDocumentValue(object documentValue, IMappingContext mappingContext)
        {
            documentValue = base.ConvertFromDocumentValue(documentValue, mappingContext);
            if (documentValue == null)
                return documentValue;

            var dbref = (DBRef)documentValue;
            //TODO: this is where we would do lazy loading/proxying
            return mappingContext.MongoContext.FindOne(this.Type, new Document().Append("_id", dbref.Id));
        }

        /// <summary>
        /// Converts to document value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns></returns>
        public override object ConvertToDocumentValue(object value, IMappingContext mappingContext)
        {
            value = base.ConvertToDocumentValue(value, mappingContext);
            if (value == MongoDBNull.Value)
            {
                //TODO: alert someone that this reference has been deleted for potential automatic deletion of referenced entity...
                return value;
            }

            var refClassMap = mappingContext.MongoContext.Configuration.MappingStore.GetClassMapFor(this.Type);
            var id = refClassMap.GetId(value);
            if (Object.Equals(id, refClassMap.IdMap.UnsavedValue))
            {
                //TODO: alert someone that this reference needs to be inserted...
            }

            var idValue = refClassMap.IdMap.ValueType.ConvertToDocumentValue(id, mappingContext);
            return new DBRef(refClassMap.CollectionName, idValue);
        }
    }
}