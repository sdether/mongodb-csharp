using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using Remotion.Data.Linq.Parsing;
using Remotion.Data.Linq.Clauses.ExpressionTreeVisitors;

using MongoDB.Driver;
using MongoDB.Framework.Mapping;

namespace MongoDB.Framework.Linq.Visitors
{
    public class QueryDocumentBuilder : ThrowingExpressionTreeVisitor
    {
        public static Document BuildFrom(IMongoContextImplementor mongoContext, Expression expression)
        {
            var builder = new QueryDocumentBuilder(mongoContext);
            builder.VisitExpression(expression);
            return builder.query;
        }

        #region Private Fields

        private Dictionary<string, Document> conditions;
        private IMongoContextImplementor mongoContext;
        private Document query;
        private MemberMapPath memberMapPath;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoWhereClauseExpressionTreeVisitor"/> class.
        /// </summary>
        /// <param name="mongoContext">The mongo context.</param>
        private QueryDocumentBuilder(IMongoContextImplementor mongoContext)
        {
            this.mongoContext = mongoContext;
            this.query = new Document();
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

            value = this.memberMapPath.ConvertToDocumentValue(value, this.mongoContext);
            if (op == "$eq")
                this.query[this.memberMapPath.Key] = value;
            else
            {
                Document doc = (Document)this.query[this.memberMapPath.Key];
                if (doc == null)
                    this.query[this.memberMapPath.Key] = doc = new Document();

                doc.Append(op, value);
            }

            return expression;
        }

        protected override Expression VisitMemberExpression(MemberExpression expression)
        {
            this.memberMapPath = MemberMapPathBuilder.BuildFrom(this.mongoContext.MappingStore, expression);
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