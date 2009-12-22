using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using MongoDB.Driver;
using Remotion.Data.Linq.Clauses;

namespace MongoDB.Framework.Linq
{
    public class MongoQuerySpecification
    {
        private Document orderBy;
        private MongoQueryProjection projection;
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

        public MongoQueryProjection Projection
        {
            get
            {
                if (this.projection == null)
                    this.projection = new MongoQueryProjection();
                return this.projection;
            }
            set { this.projection = value; }
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
    }
}