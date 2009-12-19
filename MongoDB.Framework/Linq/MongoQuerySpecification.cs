using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Linq
{
    public class MongoQuerySpecification
    {
        public bool IsCount { get; set; }

        public bool IsFindOne
        {
            get
            {
                return this.Limit == 1
                    && this.Skip == 0
                    && (this.ProjectedFields.Count == 0)
                    && (this.OrderBy.Count == 0);
            }
        }

        public int Limit { get; set; }

        public Document OrderBy { get; private set; }

        public IList<ProjectedField> ProjectedFields { get; private set; }

        public Document Query { get; private set; }

        public int Skip { get; set; }

        public MongoQuerySpecification()
        {
            this.OrderBy = new Document();
            this.ProjectedFields = new List<ProjectedField>();
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

        public Document GetFields()
        {
            Document doc = new Document();
            foreach (var projectedField in this.ProjectedFields)
                doc.Add(projectedField.DocumentKey, 1);
            return doc;
        }
    }
}