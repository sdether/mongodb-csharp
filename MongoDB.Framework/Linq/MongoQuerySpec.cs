using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Linq
{
    public class MongoQuerySpec
    {
        private Document query;
        private Document fields;
        private Document sortOrder;

        public Document Query
        {
            get
            {
                if (this.query == null)
                    this.query = new Document();
                return this.query;
            }
        }

        public int Limit { get; set; }

        public int Skip { get; set; }

        public Document Fields
        {
            get
            {
                if (this.fields == null)
                    this.fields = new Document();
                return this.fields;
            }
        }

        public Document SortOrder
        {
            get
            {
                if (this.sortOrder == null)
                    this.sortOrder = new Document();
                return this.sortOrder;
            }
        }

        public bool IsFirstCall { get; set; }

        public MongoQuerySpec()
        {
            this.Limit = 0;
            this.Skip = 0;
            this.IsFirstCall = true;
        }

    }
}