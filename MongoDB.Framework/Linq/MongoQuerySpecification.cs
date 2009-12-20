using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace MongoDB.Framework.Linq
{
    public class MongoQuerySpecification<T>
    {
        public Document Fields { get; private set; }

        public bool IsFindOne
        {
            get
            {
                return this.Limit == 1
                    && this.Skip == 0
                    && (this.Fields.Count == 0)
                    && (this.OrderBy.Count == 0);
            }
        }

        public int Limit { get; set; }

        public Document OrderBy { get; private set; }

        public Document Query { get; private set; }

        public Func<Document, T> Projector { get; set; }

        public int Skip { get; set; }

        public MongoQuerySpecification()
        {
            this.OrderBy = new Document();
            this.Fields = new Document();
            this.Query = new Document();
        }

        public Document GetCompleteQuery()
        {
            Document doc = new Document();
            doc["query"] = this.Query;

            if(this.OrderBy.Count > 0)
                doc["orderby"] = this.OrderBy;

            return doc;            
        }
    }
}