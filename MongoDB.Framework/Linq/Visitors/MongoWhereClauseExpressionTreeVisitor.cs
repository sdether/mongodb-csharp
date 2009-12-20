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

namespace MongoDB.Framework.Linq.Visitors
{
    public class MongoWhereClauseExpressionTreeVisitor : ThrowingExpressionTreeVisitor
    {
        #region Private Fields

        private Dictionary<string, Document> conditions;
        private MongoConfiguration configuration;
        private List<MemberInfo> memberPathParts;
        private Document query;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoWhereClauseExpressionTreeVisitor"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public MongoWhereClauseExpressionTreeVisitor(MongoConfiguration configuration)
        {
            this.conditions = new Dictionary<string, Document>();
            this.configuration = configuration;
            this.memberPathParts = new List<MemberInfo>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the query from.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public Document CreateQueryFrom(Expression expression)
        {
            this.query = new Document();
            this.VisitExpression(expression);
            return query;
        }

        #endregion

        #region Protected Methods

        protected override Expression VisitBinaryExpression(BinaryExpression expression)
        {
            string op = null;
            switch (expression.NodeType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    this.VisitExpression(expression.Left);
                    this.VisitExpression(expression.Right);
                    return expression;
                case ExpressionType.Equal:
                    op = "$eq";
                    break;
                case ExpressionType.GreaterThan:
                    op = "$gt";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    op = "$gte";
                    break;
                case ExpressionType.LessThan:
                    op = "$lt";
                    break;
                case ExpressionType.LessThanOrEqual:
                    op = "$lte";
                    break;
                case ExpressionType.NotEqual:
                    op = "$ne";
                    break;
                default:
                    throw new NotSupportedException(string.Format("The binary operator {0} is not supported.", expression.NodeType));
            }

            object value;
            if (expression.Left.NodeType == ExpressionType.MemberAccess &&
                expression.Right.NodeType == ExpressionType.Constant)
            {
                this.VisitExpression(expression.Left);
                value = ((ConstantExpression)expression.Right).Value;
            }
            else if (expression.Left.NodeType == ExpressionType.Constant &&
                expression.Right.NodeType == ExpressionType.MemberAccess)
            {
                this.VisitExpression(expression.Right);
                value = ((ConstantExpression)expression.Left).Value;
            }
            else
                throw new NotSupportedException();

            if (this.memberPathParts.Count == 0)
                throw new InvalidOperationException("No member path parts exist.");

            var visitor = new MemberPathToQueryConditionVisitor(this.memberPathParts, value);
            var rootEntityMap = this.configuration.GetRootEntityMapFor(this.memberPathParts[0].DeclaringType);
            rootEntityMap.Accept(visitor);

            if (op == "$eq")
                this.query[visitor.DocumentKey] = visitor.DocumentValue;
            else
            {
                Document doc = (Document)this.query[visitor.DocumentKey];
                if (doc == null)
                    this.query[visitor.DocumentKey] = doc = new Document();

                doc.Append(op, visitor.DocumentValue);
            }

            this.memberPathParts.Clear();
            
            return expression;
        }

        protected override Expression VisitMemberExpression(MemberExpression expression)
        {
            this.VisitExpression(expression.Expression);
            this.memberPathParts.Add(expression.Member);

            return expression;
        }

        protected override Expression VisitQuerySourceReferenceExpression(Remotion.Data.Linq.Clauses.Expressions.QuerySourceReferenceExpression expression)
        {
            return expression;
        }

        protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod)
        {
            var itemAsExpression = unhandledItem as Expression;
            string itemText = itemAsExpression != null ? FormattingExpressionTreeVisitor.Format(itemAsExpression) : unhandledItem.ToString();
            var message = string.Format("The expression '{0}' (type: {1}) is not supported by this LINQ provider.", itemText, typeof(T));
            return new NotSupportedException(message);
        }

        #endregion
    }
}