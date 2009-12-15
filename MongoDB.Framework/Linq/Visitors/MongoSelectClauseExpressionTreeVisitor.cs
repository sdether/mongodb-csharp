using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Driver;
using Remotion.Data.Linq.Parsing;
using Remotion.Data.Linq.Clauses.ExpressionTreeVisitors;
using MongoDB.Framework.Configuration;

namespace MongoDB.Framework.Linq.Visitors
{
    /// <summary>
    /// 
    /// </summary>
    public class MongoSelectClauseExpressionTreeVisitor : ThrowingExpressionTreeVisitor
    {
        #region Public Static Methods

        /// <summary>
        /// Populates the spec.
        /// </summary>
        /// <param name="spec">The spec.</param>
        /// <param name="expression">The expression.</param>
        public static void PopulateFields(MongoQuerySpec spec, EntityMapper entityMapper, Expression expression)
        {
            var visitor = new MongoSelectClauseExpressionTreeVisitor(entityMapper);
            visitor.VisitExpression(expression);
            visitor.keys.ForEach(f => spec.Fields.Append(f, 1));
        }

        #endregion

        #region Private Fields

        private MongoConfiguration configuration;
        private EntityMapper entityMapper;
        private List<string> keys;
        private List<MemberInfo> memberPath;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoSelectClauseExpressionTreeVisitor"/> class.
        /// </summary>
        private MongoSelectClauseExpressionTreeVisitor(EntityMapper entityMapper)
        {
            this.configuration = entityMapper.Configuration;
            this.entityMapper = entityMapper;
            this.keys = new List<string>();
            this.memberPath = new List<MemberInfo>();
        }

        #endregion

        #region Protected Methods

        protected override Expression VisitNewExpression(NewExpression expression)
        {
            foreach (var arg in expression.Arguments)
            {
                this.VisitExpression(arg);
                
            }

            return expression;
        }

        protected override Expression VisitQuerySourceReferenceExpression(Remotion.Data.Linq.Clauses.Expressions.QuerySourceReferenceExpression expression)
        {
            return expression;
        }

        protected override Expression VisitMemberExpression(MemberExpression expression)
        {
            this.VisitExpression(expression.Expression);
            this.memberPath.Add(expression.Member);

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

        #region Private Methods

        /// <summary>
        /// Creates the key from member path.
        /// </summary>
        private void CreateKeyFromMemberPath()
        {
            if (memberPath.Count == 0)
                return;

            RootEntityMap rootEntityMap = this.configuration.GetRootEntityMapFor(memberPath[0].DeclaringType);
            DiscriminatedEntityMap entityMap;
            if (rootEntityMap.Type != memberPath[0].DeclaringType)
            {
                //it is a discriminated entity.
            }
        }

        #endregion
    }
}