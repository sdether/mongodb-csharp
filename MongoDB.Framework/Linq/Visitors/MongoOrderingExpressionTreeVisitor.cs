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
    public class MongoOrderingExpressionTreeVisitor : ThrowingExpressionTreeVisitor
    {
        #region Private Fields

        private MongoConfiguration configuration;
        private List<MemberInfo> memberPathParts;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoWhereClauseExpressionTreeVisitor"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public MongoOrderingExpressionTreeVisitor(MongoConfiguration configuration)
        {
            this.configuration = configuration;
            this.memberPathParts = new List<MemberInfo>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the document key from.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public string GetDocumentKeyFrom(Expression expression)
        {
            this.VisitExpression(expression);
            return this.CreateDocumentKeyFromMemberPathParts();
        }

        #endregion

        #region Protected Methods

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
            var visitor = new MemberPathToDocumentKeyVisitor(this.memberPathParts);
            var rootEntityMap = this.configuration.GetRootEntityMapFor(this.memberPathParts[0].DeclaringType);
            rootEntityMap.Accept(visitor);
            return visitor.DocumentKey;
        }

        #endregion
    }
}