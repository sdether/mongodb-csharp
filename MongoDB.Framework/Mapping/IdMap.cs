using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Framework.Mapping.Types;

namespace MongoDB.Framework.Mapping
{
    public class IdMap : MemberMap
    {
        public IdMap()
        {
            this.Key = "_id";
            this.ValueType = new IdValueType();
        }

        public override void TranslateToDocument(MappingContext mappingContext)
        {
            base.TranslateToDocument(mappingContext);
            if (mappingContext.Document[this.Key] == MongoDBNull.Value)
                mappingContext.Document.Remove(this.Key);
        }
    }
}