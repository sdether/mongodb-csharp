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

        public override void TranslateToDocument(TranslationContext translationContext)
        {
            base.TranslateToDocument(translationContext);
            if (translationContext.Document[this.Key] == MongoDBNull.Value)
                translationContext.Document.Remove(this.Key);
        }
    }
}