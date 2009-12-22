using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using Remotion.Data.Linq.Parsing;
using Remotion.Data.Linq.Clauses.ExpressionTreeVisitors;

using MongoDB.Driver;

namespace MongoDB.Framework.Linq
{
    public class MongoQueryProjection
    {
        private Document fields;
        private Func<ResultObjectMapping, object> projector;

        public Document Fields
        {
            get
            {
                if (this.fields == null)
                    this.fields = new Document();
                return this.fields;
            }
            set { this.fields = value; }
        }

        public Func<ResultObjectMapping, object> Projector
        {
            get { return this.projector; }
            set { this.projector = value; }
        }

    }
}