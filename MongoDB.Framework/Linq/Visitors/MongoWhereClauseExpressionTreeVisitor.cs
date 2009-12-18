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

        public static void ModifyQuery(Document query, MongoConfiguration configuration, Expression expression)
        {
            var visitor = new MongoWhereClauseExpressionTreeVisitor(configuration, query);
            visitor.VisitExpression(expression);
        }

        #endregion

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
        private MongoWhereClauseExpressionTreeVisitor(MongoConfiguration configuration, Document query)
        {
            this.conditions = new Dictionary<string, Document>();
            this.configuration = configuration;
            this.memberPathParts = new List<MemberInfo>();
            this.query = query;
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

            string key;
            object value;
            if (expression.Left.NodeType == ExpressionType.MemberAccess &&
                expression.Right.NodeType == ExpressionType.Constant)
            {
                this.VisitExpression(expression.Left);
                key = this.CreateDocumentKeyFromMemberPathParts();
                value = ((ConstantExpression)expression.Right).Value;
            }
            else if (expression.Left.NodeType == ExpressionType.Constant &&
                expression.Right.NodeType == ExpressionType.MemberAccess)
            {
                this.VisitExpression(expression.Right);
                key = this.CreateDocumentKeyFromMemberPathParts();
                value = ((ConstantExpression)expression.Left).Value;
            }
            else
                throw new NotSupportedException();

            
            if (op == "$eq")
                this.query[key] = value;
            else
            {
                Document doc = (Document)this.query[key];
                if (doc == null)
                    this.query[key] = doc = new Document();

                doc.Append(op, value);
            }
            
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
            //if(this.memberPathParts.Count == 0)
            //    throw new InvalidOperationException("No member path parts exist.");

            //var memberInfo = this.memberPathParts[0];
            //EntityMap entityMap = this.configuration.GetRootEntityMapFor(this.memberPathParts[0].DeclaringType);
            //var memberMap = entityMap.GetMemberMap(memberInfo.DeclaringType, memberInfo.Name);
            //string key = memberMap.DocumentKey;
            //for (int i = 1; i < this.memberPathParts.Count; i++)
            //{
            //    var entityMemberMap = memberMap as ComponentMap;
            //    if (entityMemberMap == null)
            //        throw new UnmappedMemberException(string.Format("{0}.{1} is unmapped.", this.memberPathParts[i].DeclaringType, this.memberPathParts[i].Name));

            //    entityMap = entityMemberMap.EntityMap;
            //    memberMap = entityMap.GetMemberMap(this.memberPathParts[i].DeclaringType, this.memberPathParts[i].Name);
            //    key += "." + memberMap.DocumentKey;
            //}

            //this.memberPathParts.Clear();

            //return key;
            return "blah";
        }

        #endregion
    }
}