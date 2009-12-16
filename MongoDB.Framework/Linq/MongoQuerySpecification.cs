using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Linq
{
    public class MongoQuerySpecification
    {
        private Document projection;
        private Document query;
        private Document orderBy;

        public bool IsCount { get; set; }

        public bool IsSingle
        {
            get
            {
                return this.Limit == 1
                    && this.Skip == 0
                    && (this.projection == null || this.orderBy.Count == 0)
                    && (this.orderBy == null || this.orderBy.Count == 0);
            }
        }

        public int Limit { get; set; }

        public Document Projection
        {
            get
            {
                if (this.projection == null)
                    this.projection = new Document();
                return this.projection;
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

        public Document OrderBy
        {
            get
            {
                if (this.orderBy == null)
                    this.orderBy = new Document();
                return this.orderBy;
            }
        }

        public int Skip { get; set; }

        public MongoQuerySpecification()
        {
        }
    }
}