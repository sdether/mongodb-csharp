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
    public class MongoOrderingExpressionTreeVisitor : ThrowingExpressionTreeVisitor
    {
        #region Public Static Methods

        public static string GetDocumentKey(MongoConfiguration configuration, Expression expression)
        {
            var visitor = new MongoOrderingExpressionTreeVisitor(configuration);
            visitor.VisitExpression(expression);
            return visitor.CreateDocumentKeyFromMemberPathParts();
        }

        #endregion

        #region Private Fields

        private MongoConfiguration configuration;
        private List<MemberInfo> memberPathParts;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoWhereClauseExpressionTreeVisitor"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        private MongoOrderingExpressionTreeVisitor(MongoConfiguration configuration)
        {
            this.configuration = configuration;
            this.memberPathParts = new List<MemberInfo>();
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

            this.memberPathParts.Clear();

            return key;
        }

        #endregion
    }
}