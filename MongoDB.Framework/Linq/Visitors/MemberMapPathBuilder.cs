﻿using System;
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
    public class MemberMapPathBuilder : ThrowingExpressionTreeVisitor
    {
        public static IList<MemberMap> Build(MongoConfiguration configuration, Expression expression)
        {
            var builder = new MemberMapPathBuilder(configuration);
            builder.VisitExpression(expression);
            var visitor = new MemberPathToMemberMapPathVisitor(builder.memberPath);
            var rootEntityMap = configuration.GetRootEntityMapFor(builder.memberPath[0].DeclaringType);
            rootEntityMap.Accept(visitor);
            return visitor.MemberMapPath;
        }

        #region Private Fields

        private MongoConfiguration configuration;
        private List<MemberInfo> memberPath;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoWhereClauseExpressionTreeVisitor"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        private MemberMapPathBuilder(MongoConfiguration configuration)
        {
            this.configuration = configuration;
            this.memberPath = new List<MemberInfo>();
        }

        #endregion

        #region Protected Methods

        protected override Expression VisitMemberExpression(MemberExpression expression)
        {
            this.VisitExpression(expression.Expression);
            this.memberPath.Add(expression.Member);

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