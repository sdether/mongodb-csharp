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

namespace MongoDB.Framework.Linq.Visitors
{
    public class MongoWhereClauseExpressionTreeVisitor : ThrowingExpressionTreeVisitor
    {
        #region Public Static Methods

        public static void PopulateSpec(MongoQuerySpec spec, MongoConfiguration configuration, Expression expression)
        {
            var visitor = new MongoWhereClauseExpressionTreeVisitor(spec, configuration);
            visitor.VisitExpression(expression);
        }

        #endregion

        #region Private Fields

        private MongoConfiguration configuration;
        private MongoQuerySpec spec;
        private List<MemberInfo> memberPathParts;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoWhereClauseExpressionTreeVisitor"/> class.
        /// </summary>
        /// <param name="spec">The spec.</param>
        /// <param name="configuration">The configuration.</param>
        private MongoWhereClauseExpressionTreeVisitor(MongoQuerySpec spec, MongoConfiguration configuration)
        {
            this.configuration = configuration;
            this.spec = spec;
            this.memberPathParts = new List<MemberInfo>();
        }

        #endregion

        #region Protected Methods

        protected override Expression VisitBinaryExpression(BinaryExpression expression)
        {
            if (expression.Left.NodeType != ExpressionType.MemberAccess)
                throw new NotSupportedException("MemberAccess expression must appear on the left side of a binary statement.");
            this.VisitExpression(expression.Left);
            string key = this.CreateDocumentKeyFromMemberPathParts();

            if (expression.Right.NodeType != ExpressionType.Constant)
                throw new NotSupportedException("Constant expression must appear on the right side of a binary statement.");

            object value = ((ConstantExpression)expression.Right).Value;

            string op = null;
            switch (expression.NodeType)
            {
                case ExpressionType.Equal:
                    this.spec.Query.Add(key, value);
                    return expression;
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
                    throw new NotSupportedException();
            }

            this.spec.Query.Append(key, new Document().Append(op, value));
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

        #region Private Fields

        private string CreateDocumentKeyFromMemberPathParts()
        {
            if(this.memberPathParts.Count == 0)
                throw new InvalidOperationException("No member path parts exist.");

            var memberInfo = this.memberPathParts[0];
            EntityMap entityMap = this.configuration.GetRootEntityMapFor(this.memberPathParts[0].DeclaringType);
            var memberMap = entityMap.GetMemberMap(memberInfo.DeclaringType, memberInfo.Name);
            string key = memberMap.DocumentKey;
            for (int i = 1; i < this.memberPathParts.Count; i++)
            {
                var entityMemberMap = memberMap as EntityMemberMap;
                if (entityMemberMap == null)
                    throw new UnmappedMemberException(string.Format("{0}.{1} is unmapped.", this.memberPathParts[i].DeclaringType, this.memberPathParts[i].Name));

                entityMap = entityMemberMap.EntityMap;
                memberMap = entityMap.GetMemberMap(this.memberPathParts[i].DeclaringType, this.memberPathParts[i].Name);
                key += "." + memberMap.DocumentKey;
            }

            return key;
        }

        #endregion
    }
}