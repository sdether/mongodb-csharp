using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using Remotion.Data.Linq.Parsing;
using Remotion.Data.Linq.Clauses.ExpressionTreeVisitors;

using MongoDB.Framework.Configuration.Mapping;

namespace MongoDB.Framework.Linq.Visitors
{
    public class MemberMapPathBuilder : ThrowingExpressionTreeVisitor
    {
        #region Public Static Methods

        public static MemberMapPath BuildFrom(IMappingStore mappingStore, Expression expression)
        {
            MemberMapPathBuilder builder = new MemberMapPathBuilder(mappingStore);
            builder.VisitExpression(expression);
            return builder.memberMapPath;
        }

        #endregion

        #region Private Fields

        private IMappingStore mappingStore;
        private Stack<MemberInfo> members = new Stack<MemberInfo>();
        private MemberMapPath memberMapPath;

        #endregion

        #region Constructors

        private MemberMapPathBuilder(IMappingStore mappingStore)
        {
            this.mappingStore = mappingStore;
        }

        #endregion

        #region Public Methods

        protected override Expression VisitMemberExpression(MemberExpression expression)
        {
            this.members.Push(expression.Member);
            this.VisitExpression(expression.Expression);
            return expression;
        }

        protected override Expression VisitQuerySourceReferenceExpression(Remotion.Data.Linq.Clauses.Expressions.QuerySourceReferenceExpression expression)
        {
            var classMap = this.mappingStore.GetClassMapFor(expression.ReferencedQuerySource.ItemType);
            this.memberMapPath = new MemberMapPath(this.mappingStore, expression.ReferencedQuerySource.ItemType, this.members);
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