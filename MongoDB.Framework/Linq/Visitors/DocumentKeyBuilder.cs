using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using Remotion.Data.Linq.Parsing;
using Remotion.Data.Linq.Clauses.ExpressionTreeVisitors;

using MongoDB.Framework.Mapping;

namespace MongoDB.Framework.Linq.Visitors
{
    public class DocumentKeyBuilder : ThrowingExpressionTreeVisitor
    {
        #region Public Static Methods

        public static string BuildFrom(MappingStore mappingStore, Expression expression)
        {
            DocumentKeyBuilder builder = new DocumentKeyBuilder(mappingStore);
            builder.VisitExpression(expression);
            return builder.documentKey;
        }

        #endregion

        #region Private Fields

        private MappingStore mappingStore;
        private Stack<MemberInfo> members = new Stack<MemberInfo>();
        private string documentKey;

        #endregion

        #region Constructors

        private DocumentKeyBuilder(MappingStore mappingStore)
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
            var documentMap = this.mappingStore.GetDocumentMapFor(expression.ReferencedQuerySource.ItemType);
            var memberInfo = this.members.Pop();
            var valueMap = documentMap.GetValueMapFromMemberName(memberInfo.Name);
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