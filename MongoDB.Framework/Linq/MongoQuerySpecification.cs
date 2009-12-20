using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using System.Linq.Expressions;
using MongoDB.Framework.Configuration;
using MongoDB.Framework.Configuration.Visitors;

namespace MongoDB.Framework.Linq
{
    public class MongoQuerySpecification<T>
    {
        private Document orderBy;
        private MongoQueryProjection<T> projection;
        private Document query;

        public bool IsFindOne
        {
            get
            {
                return this.Limit == 1
                    && this.Skip == 0
                    && (this.Projection.Fields.Count == 0)
                    && (this.OrderBy.Count == 0);
            }
        }

        public int Limit { get; set; }

        /// <summary>
        /// Gets or sets the order by.
        /// </summary>
        /// <value>The order by.</value>
        public Document OrderBy
        {
            get
            {
                if (this.orderBy == null)
                    this.orderBy = new Document();
                return this.orderBy;
            }
            set { this.orderBy = value; }
        }

        public MongoQueryProjection<T> Projection
        {
            get
            {
                if (this.projection == null)
                    this.projection = new MongoQueryProjection<T>();
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
            set { this.query = value; }
        }

        public int Skip { get; set; }

        public Document GetCompleteQuery()
        {
            Document doc = new Document();
            doc["query"] = this.Query;

            if (this.OrderBy.Count > 0)
                doc["orderby"] = this.OrderBy;

            return doc;
        }
    }
}