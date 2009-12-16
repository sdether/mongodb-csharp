using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using Remotion.Data.Linq;
using Remotion.Data.Linq.Clauses;
using MongoDB.Framework.Configuration;
using Remotion.Data.Linq.Clauses.ResultOperators;
using System.Linq.Expressions;

namespace MongoDB.Framework.Linq.Visitors
{
    public class MongoQueryModelVisitor : QueryModelVisitorBase
    {
        #region Public Static Methods

        /// <summary>
        /// Creates the mongo query specification.
        /// </summary>
        /// <param name="queryModel">The query model.</param>
        /// <param name="entityMapper">The entity mapper.</param>
        /// <returns></returns>
        public static MongoQuerySpecification CreateMongoQuerySpecification(QueryModel queryModel, MongoConfiguration configuration)
        {
            if (queryModel == null)
                throw new ArgumentNullException("queryModel");
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            var visitor = new MongoQueryModelVisitor(configuration);
            visitor.VisitQueryModel(queryModel);
            return visitor.querySpec;
        }

        #endregion

        #region Private Fields

        private MongoConfiguration configuration;
        private MongoQuerySpecification querySpec;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoQueryModelVisitor"/> class.
        /// </summary>
        /// <param name="entityMapper">The entity mapper.</param>
        private MongoQueryModelVisitor(MongoConfiguration configuration)
        {
            this.configuration = configuration;
            this.querySpec = new MongoQuerySpecification();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Visits the query model.
        /// </summary>
        /// <param name="queryModel">The query model.</param>
        public override void VisitQueryModel(QueryModel queryModel)
        {
            queryModel.SelectClause.Accept(this, queryModel);
            this.VisitBodyClauses(queryModel.BodyClauses, queryModel);
            this.VisitResultOperators(queryModel.ResultOperators, queryModel);
        }

        /// <summary>
        /// Visits the result operator.
        /// </summary>
        /// <param name="resultOperator">The result operator.</param>
        /// <param name="queryModel">The query model.</param>
        /// <param name="index">The index.</param>
        public override void VisitResultOperator(ResultOperatorBase resultOperator, QueryModel queryModel, int index)
        {
            if (resultOperator is SkipResultOperator)
            {
                if (index > 0 && this.querySpec.Limit > 0)
                    throw new NotSupportedException("Skip operators must come before Take operators.");

                var constantExpression = ((SkipResultOperator)resultOperator).Count as ConstantExpression;
                if (constantExpression == null)
                    throw new NotSupportedException("Only constant skip counts are supported.");
                this.querySpec.Skip = (int)constantExpression.Value;
            }
            else if (resultOperator is TakeResultOperator)
            {
                var constantExpression = ((TakeResultOperator)resultOperator).Count as ConstantExpression;
                if (constantExpression == null)
                    throw new NotSupportedException("Only constant take counts are supported.");
                this.querySpec.Limit = (int)constantExpression.Value;
            }
            else if (resultOperator is CountResultOperator)
            {
                this.querySpec.IsCount = true;
            }
            else
                throw new NotSupportedException(string.Format("Operator {0} is not supported.", resultOperator.GetType()));
        }

        /// <summary>
        /// Visits the where clause.
        /// </summary>
        /// <param name="whereClause">The where clause.</param>
        /// <param name="queryModel">The query model.</param>
        /// <param name="index">The index.</param>
        public override void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index)
        {
            MongoWhereClauseExpressionTreeVisitor.ModifyQuery(this.querySpec.Query, this.configuration, whereClause.Predicate);
        }

        #endregion
    }
}