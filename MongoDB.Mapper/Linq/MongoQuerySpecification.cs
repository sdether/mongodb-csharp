using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using MongoDB.Driver;
using Remotion.Data.Linq.Clauses;

namespace MongoDB.Mapper.Linq
{
    public class MongoQuerySpecification
    {
        private Document orderBy;
        private MongoQueryProjection projection;
        private Document conditions;

        public Document Conditions
        {
            get
            {
                if (this.conditions == null)
                    this.conditions = new Document();
                return this.conditions;
            }
            set { this.conditions = value; }
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

        public int Skip { get; set; }
    }
}