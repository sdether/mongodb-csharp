using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Linq
{
    public class MongoQuerySpecification
    {
        private Document fields;
        private Document query;
        private Document orderBy;

        public bool IsCount { get; set; }

        public bool IsFindOne
        {
            get
            {
                return this.Limit == 1
                    && this.Skip == 0
                    && (this.fields == null || this.fields.Count == 0)
                    && (this.orderBy == null || this.orderBy.Count == 0);
            }
        }

        public int Limit { get; set; }

        public Document Fields
        {
            get
            {
                if (this.fields == null)
                    this.fields = new Document();
                return this.fields;
            }
        }
        public Document OrderBy
        {
            get
            {
                if (this.orderBy == null)
                    this.orderBy = new Document();
                return this.orderBy;
            }
        }

        public Document Query
        {
            get
            {
                if (this.query == null)
                    this.query = new Document();
                return this.query;
            }
        }


        public int Skip { get; set; }

        public MongoQuerySpecification()
        {
        }

        public Document GetCompleteQuery()
        {
            Document doc = new Document();
            doc["query"] = this.Query;

            if(this.orderBy != null)
                doc["orderby"] = this.orderBy;

            return doc;            
        }
    }
}