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
using Remotion.Data.Linq.Clauses;
using Remotion.Data.Linq.Clauses.Expressions;

namespace MongoDB.Framework.Linq.Visitors
{
    public class MongoProjectionExpressionTreeVisitor : ThrowingExpressionTreeVisitor
    {
        #region Private Static Fields

        private static MethodInfo resolveValueMethodInfo =
            typeof(SingleDocumentValueResolver).GetMethod("ResolveValue");

        #endregion

        #region Private Fields

        private MongoConfiguration configuration;
        private ParameterExpression documentParameter;
        private Document fields;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoWhereClauseExpressionTreeVisitor"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public MongoProjectionExpressionTreeVisitor(MongoConfiguration configuration)
        {
            this.configuration = configuration;
            this.documentParameter = Expression.Parameter(typeof(Document), "document");
            this.fields = new Document();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the document key from.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public Projection<T> GetProjection<T>(Expression expression)
        {
            var body = this.VisitExpression(expression);

            var projector = Expression.Lambda<Func<Document, T>>(body, this.documentParameter);

            return new Projection<T>(projector.Compile(), this.fields);
        }

        #endregion

        #region Protected Methods

        protected override Expression VisitMemberExpression(MemberExpression expression)
        {
            var visitor = new MongoMemberMapPathExpressionTreeVisitor(this.configuration);
            var memberMapPath = visitor.GetMemberMapPath(expression);
            this.fields.Add(string.Join(".", memberMapPath.Select(mm => mm.DocumentKey).ToArray()), 1);

            var resolver = Activator.CreateInstance(typeof(SingleDocumentValueResolver), memberMapPath);
            var method = resolveValueMethodInfo.MakeGenericMethod(expression.Type);
            return Expression.Call(Expression.Constant(resolver), method, this.documentParameter);
        }

        protected override Expression VisitNewExpression(NewExpression expression)
        {
            var argumentExpressions = new List<Expression>();
            foreach (var argument in expression.Arguments)
                argumentExpressions.Add(this.VisitExpression(argument));

            return Expression.New(
                expression.Constructor,
                argumentExpressions,
                expression.Members);
        }

        protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod)
        {
            var itemAsExpression = unhandledItem as Expression;
            string itemText = itemAsExpression != null ? FormattingExpressionTreeVisitor.Format(itemAsExpression) : unhandledItem.ToString();
            var message = string.Format("The expression '{0}' (type: {1}) is not supported by this LINQ provider.", itemText, typeof(T));
            return new NotSupportedException(message);
        }

        #endregion

        #region Private Class : DocumentValueResolver

        private class SingleDocumentValueResolver
        {
            private List<MemberMap> memberMapPath;

            public SingleDocumentValueResolver(IEnumerable<MemberMap> memberMapPath)
            {
                this.memberMapPath = new List<MemberMap>(memberMapPath);
            }

            public T ResolveValue<T>(Document document)
            {
                object value = null;
                for (int i = 0; i < this.memberMapPath.Count - 1; i++)
                {
                    value = document[memberMapPath[i].DocumentKey];
                    if (value is Document)
                        document = (Document)value;
                }

                var lastMemberMap = memberMapPath[memberMapPath.Count - 1];
                var componentMemberMap = lastMemberMap as ComponentMemberMap;
                if (componentMemberMap != null)
                {
                    var visitor = new DocumentToEntityTranslator((Document)document[componentMemberMap.DocumentKey]);
                    componentMemberMap.EntityMap.Accept(visitor);
                    return (T)visitor.Entity;
                }
                else
                {
                    return (T)lastMemberMap.GetValueFromDocument(document);
                }
            }
        }

        #endregion
    }
}