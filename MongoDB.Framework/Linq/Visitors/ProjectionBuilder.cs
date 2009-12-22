using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using Remotion.Data.Linq.Parsing;
using Remotion.Data.Linq.Clauses;
using Remotion.Data.Linq.Clauses.Expressions;
using Remotion.Data.Linq.Clauses.ExpressionTreeVisitors;

using MongoDB.Driver;

namespace MongoDB.Framework.Linq.Visitors
{
    //public class ProjectionBuilder : ThrowingExpressionTreeVisitor
    //{
    //    public static MongoQueryProjection<T> Build<T>(MongoConfiguration configuration, Expression expression)
    //    {
    //        var configurationParameter = Expression.Parameter(typeof(MongoConfiguration), "configuration");
    //        var documentParameter = Expression.Parameter(typeof(Document), "document");
    //        ProjectionBuilder builder = new ProjectionBuilder(configuration, configurationParameter, documentParameter);

    //        var body = builder.VisitExpression(expression);

    //        var projector = Expression.Lambda<Func<MongoConfiguration, Document, T>>(body, configurationParameter, documentParameter);

    //        var projection = new MongoQueryProjection<T>();
    //        projection.Fields = builder.fields;
    //        projection.Projector = projector.Compile();
    //        return projection;
    //    }

    //    #region Private Static Fields

    //    private static MethodInfo resolveValueMethodInfo =
    //        typeof(SingleDocumentValueResolver).GetMethod("ResolveValue");

    //    #endregion

    //    #region Private Fields

    //    private MongoConfiguration configuration;
    //    private ParameterExpression configurationParameter;
    //    private ParameterExpression documentParameter;
    //    private Document fields;

    //    #endregion

    //    #region Constructors

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="MongoWhereClauseExpressionTreeVisitor"/> class.
    //    /// </summary>
    //    /// <param name="configuration">The configuration.</param>
    //    private ProjectionBuilder(MongoConfiguration configuration, ParameterExpression configurationParameter, ParameterExpression documentParameter)
    //    {
    //        this.configuration = configuration;
    //        this.configurationParameter = configurationParameter;
    //        this.documentParameter = documentParameter;
    //        this.fields = new Document();
    //    }

    //    #endregion

    //    #region Protected Methods

    //    protected override Expression VisitQuerySourceReferenceExpression(QuerySourceReferenceExpression expression)
    //    {
    //        return base.VisitQuerySourceReferenceExpression(expression);
    //    }

    //    protected override Expression VisitMemberExpression(MemberExpression expression)
    //    {
    //        var memberMapPath = MemberMapPathBuilder.Build(this.configuration, expression);
    //        this.fields.Add(string.Join(".", memberMapPath.Select(mm => mm.DocumentKey).ToArray()), 1);

    //        var resolver = Activator.CreateInstance(typeof(SingleDocumentValueResolver), memberMapPath);
    //        var method = resolveValueMethodInfo.MakeGenericMethod(expression.Type);
    //        return Expression.Call(Expression.Constant(resolver), method, this.configurationParameter, this.documentParameter);
    //    }

    //    protected override Expression VisitNewExpression(NewExpression expression)
    //    {
    //        var argumentExpressions = new List<Expression>();
    //        foreach (var argument in expression.Arguments)
    //            argumentExpressions.Add(this.VisitExpression(argument));

    //        return Expression.New(
    //            expression.Constructor,
    //            argumentExpressions,
    //            expression.Members);
    //    }

    //    protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod)
    //    {
    //        var itemAsExpression = unhandledItem as Expression;
    //        string itemText = itemAsExpression != null ? FormattingExpressionTreeVisitor.Format(itemAsExpression) : unhandledItem.ToString();
    //        var message = string.Format("The expression '{0}' (type: {1}) is not supported by this LINQ provider.", itemText, typeof(T));
    //        return new NotSupportedException(message);
    //    }

    //    #endregion

    //    #region Private Class : SingleDocumentValueResolver

    //    private class SingleDocumentValueResolver
    //    {
    //        private List<MemberMap> memberMapPath;

    //        public SingleDocumentValueResolver(IEnumerable<MemberMap> memberMapPath)
    //        {
    //            this.memberMapPath = new List<MemberMap>(memberMapPath);
    //        }

    //        public T ResolveValue<T>(MongoConfiguration configuration, Document document)
    //        {
    //            object value = null;
    //            for (int i = 0; i < this.memberMapPath.Count - 1; i++)
    //            {
    //                value = document[memberMapPath[i].DocumentKey];
    //                if (value is Document)
    //                    document = (Document)value;
    //            }

    //            var lastMemberMap = memberMapPath[memberMapPath.Count - 1];
    //            var componentMemberMap = lastMemberMap as ComponentMemberMap;
    //            if (componentMemberMap != null)
    //            {
    //                var visitor = new DocumentToEntityTranslator((Document)document[componentMemberMap.DocumentKey]);
    //                componentMemberMap.EntityMap.Accept(visitor);
    //                return (T)visitor.Entity;
    //            }
    //            else
    //            {
    //                return (T)lastMemberMap.GetValueFromDocument(document);
    //            }
    //        }
    //    }

    //    #endregion
    //}
}