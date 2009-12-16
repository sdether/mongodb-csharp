using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Linq
{
    public class MongoQuerySpecification
    {
        public bool IsFirstCall { get; set; }

        public int Limit { get; set; }

        public Document Projection { get; set; }

        public Document Query { get; set; }

        public Document OrderBy { get; set; }

        public int Skip { get; set; }

        public MongoQuerySpecification()
        {
            this.Limit = 0;
            this.Skip = 0;
            this.IsFirstCall = true;
        }
    }
}