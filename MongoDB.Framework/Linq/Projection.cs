using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Remotion.Data.Linq.Parsing;
using Remotion.Data.Linq.Clauses.ExpressionTreeVisitors;
using MongoDB.Driver;
using MongoDB.Framework.Configuration;
using MongoDB.Framework.Configuration.Visitors;

namespace MongoDB.Framework.Linq
{
    public class Projection<T>
    {
        public Func<Document, T> Projector { get; private set; }

        public Document Fields { get; private set; }

        public Projection(Func<Document, T> projector, Document fields)
        {
            if (projector == null)
                throw new ArgumentNullException("projector");
            if (fields == null)
                throw new ArgumentNullException("fields");

            this.Fields = fields;
            this.Projector = projector;
        }
    }
}
