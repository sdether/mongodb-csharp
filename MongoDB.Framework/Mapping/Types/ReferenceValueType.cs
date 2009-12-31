using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;

namespace MongoDB.Framework.Mapping.Types
{
    public class ReferenceValueType : NullSafeValueType
    {
        public Cascade Cascade { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceValueType"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public ReferenceValueType(Type type, Cascade cascade)
            : base(type)
        {
            this.Cascade = cascade;
        }

        /// <summary>
        /// Converts from document value.
        /// </summary>
        /// <param name="documentValue">The document value.</param>
        /// <param name="mongoContext">The mongo context.</param>
        /// <returns></returns>
        public override object ConvertFromDocumentValue(object documentValue, IMongoContext mongoContext)
        {
            documentValue = base.ConvertFromDocumentValue(documentValue, mongoContext);
            if (documentValue == null)
                return documentValue;

            var dbref = (DBRef)documentValue;
            //TODO: this is where we would do lazy loading/proxying
            return mongoContext.FindOne(this.Type, new Document().Append("_id", dbref.Id));
        }

        /// <summary>
        /// Converts to document value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="mongoContext">The mongo context.</param>
        /// <returns></returns>
        public override object ConvertToDocumentValue(object value, IMongoContext mongoContext)
        {
            value = base.ConvertToDocumentValue(value, mongoContext);
            if (value == MongoDBNull.Value)
                return value;

            var refClassMap = mongoContext.Configuration.MappingStore.GetClassMapFor(this.Type);
            var id = refClassMap.GetId(value);
            var idValue = refClassMap.IdMap.ValueType.ConvertToDocumentValue(id, mongoContext);
            return new DBRef(refClassMap.CollectionName, idValue);
        }
    }
}