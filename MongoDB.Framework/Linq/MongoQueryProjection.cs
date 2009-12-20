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
    public class MongoQueryProjection<T>
    {
        private static Func<MongoConfiguration, Document, T> DefaultProjector = (c, d) =>
        {
            var rootEntityMap = c.GetRootEntityMapFor<T>();
            var translator = new DocumentToEntityTranslator(d);
            rootEntityMap.Accept(translator);
            return (T)translator.Entity;
        };

        private Document fields;
        private Func<MongoConfiguration, Document, T> projector;

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

        public Func<MongoConfiguration, Document, T> Projector
        {
            get
            {
                if (this.projector == null)
                    this.projector = DefaultProjector;
                return this.projector;
            }
            set { this.projector = value; }
        }

    }
}