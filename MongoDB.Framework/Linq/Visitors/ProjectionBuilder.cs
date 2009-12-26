using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using Remotion.Data.Linq.Parsing;
using Remotion.Data.Linq.Clauses;
using Remotion.Data.Linq.Clauses.Expressions;
using Remotion.Data.Linq.Clauses.ExpressionTreeVisitors;

using MongoDB.Driver;
using MongoDB.Framework.Mapping;

namespace MongoDB.Framework.Linq.Visitors
{
    public class ProjectionBuilder : ExpressionTreeVisitor
    {
        public static MongoQueryProjection Build(MappingStore mappingStore, Expression expression)
        {
            var resultObjectMappingParameter = Expression.Parameter(typeof(ResultObjectMapping), "resultObjectMapping");
            ProjectionBuilder builder = new ProjectionBuilder(mappingStore, resultObjectMappingParameter);

            var body = builder.VisitExpression(expression);

            var projector = Expression.Lambda<Func<ResultObjectMapping, object>>(body, resultObjectMappingParameter);

            var projection = new MongoQueryProjection();
            projection.Fields = builder.fields;
            projection.Projector = projector.Compile();
            return projection;
        }

        #region Private Static Fields

        private static MethodInfo getObjectMethod =
            typeof(ResultObjectMapping).GetMethod("GetObject");

        #endregion

        #region Private Fields

        private MappingStore mappingStore;
        private ParameterExpression resultObjectMappingParameter;
        private Document fields;
        private Stack<string> memberNames;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoWhereClauseExpressionTreeVisitor"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        private ProjectionBuilder(MappingStore mappingStore, ParameterExpression documentParameter)
        {
            this.mappingStore = mappingStore;
            this.resultObjectMappingParameter = documentParameter;
            this.fields = new Document();
            this.memberNames = new Stack<string>();
        }

        #endregion

        #region Protected Methods

        protected override Expression VisitMemberExpression(MemberExpression expression)
        {
            this.memberNames.Push(expression.Member.Name);
            return base.VisitMemberExpression(expression);
        }

        protected override Expression VisitQuerySourceReferenceExpression(QuerySourceReferenceExpression expression)
        {
            if (this.memberNames.Count != 0)
            {
                var memberMapPath = new MemberMapPath(this.mappingStore, expression.ReferencedQuerySource.ItemType, this.memberNames);
                this.memberNames.Clear();
                this.fields[memberMapPath.Key] = 1;
            }
            var method = getObjectMethod.MakeGenericMethod(expression.Type);
            return Expression.Call(
                this.resultObjectMappingParameter,
                method,
                Expression.Constant(expression.ReferencedQuerySource));
        }

        //protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod)
        //{
        //    var itemAsExpression = unhandledItem as Expression;
        //    string itemText = itemAsExpression != null ? FormattingExpressionTreeVisitor.Format(itemAsExpression) : unhandledItem.ToString();
        //    var message = string.Format("The expression '{0}' (type: {1}) is not supported by this LINQ provider.", itemText, typeof(T));
        //    return new NotSupportedException(message);
        //}

        #endregion
    }
}