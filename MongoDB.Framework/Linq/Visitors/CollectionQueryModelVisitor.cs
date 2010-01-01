using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Remotion.Data.Linq;
using Remotion.Data.Linq.Clauses;
using Remotion.Data.Linq.Clauses.ResultOperators;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;
using MongoDB.Framework.Mapping;

namespace MongoDB.Framework.Linq.Visitors
{
    public class CollectionQueryModelVisitor : QueryModelVisitorBase
    {
        #region Public Static Methods

        /// <summary>
        /// Creates the mongo query specification.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="hydrator">The hydrator.</param>
        /// <param name="queryModel">The query model.</param>
        /// <returns></returns>
        public static MongoQuerySpecification CreateMongoQuerySpecification(IMongoContext mongoContext, QueryModel queryModel)
        {
            if (mongoContext == null)
                throw new ArgumentNullException("mongoContext");
            if (queryModel == null)
                throw new ArgumentNullException("queryModel");

            var visitor = new CollectionQueryModelVisitor(mongoContext);
            visitor.VisitQueryModel(queryModel);
            return visitor.querySpec;
        }

        #endregion

        #region Private Fields

        private IMongoContext mongoContext;
        private MongoQuerySpecification querySpec;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoQueryModelVisitor"/> class.
        /// </summary>
        /// <param name="mongoContext">The mongo context.</param>
        private CollectionQueryModelVisitor(IMongoContext mongoContext)
        {
            this.mongoContext = mongoContext;
            this.querySpec = new MongoQuerySpecification();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Visits the order by clause.
        /// </summary>
        /// <param name="orderByClause">The order by clause.</param>
        /// <param name="queryModel">The query model.</param>
        /// <param name="index">The index.</param>
        public override void VisitOrderByClause(OrderByClause orderByClause, QueryModel queryModel, int index)
        {
            this.VisitOrderings(orderByClause.Orderings, queryModel, orderByClause);
        }

        /// <summary>
        /// Visits the ordering.
        /// </summary>
        /// <param name="ordering">The ordering.</param>
        /// <param name="queryModel">The query model.</param>
        /// <param name="orderByClause">The order by clause.</param>
        /// <param name="index">The index.</param>
        public override void VisitOrdering(Ordering ordering, QueryModel queryModel, OrderByClause orderByClause, int index)
        {
            var memberMapPath = MemberMapPathBuilder.BuildFrom(this.mongoContext.Configuration.IMappingStore, ordering.Expression);
            this.querySpec.OrderBy[memberMapPath.Key] = ordering.OrderingDirection == OrderingDirection.Asc ? 1 : -1;
        }

        /// <summary>
        /// Visits the query model.
        /// </summary>
        /// <param name="queryModel">The query model.</param>
        public override void VisitQueryModel(QueryModel queryModel)
        {
            this.VisitBodyClauses(queryModel.BodyClauses, queryModel);
            this.VisitResultOperators(queryModel.ResultOperators, queryModel);
            queryModel.SelectClause.Accept(this, queryModel);
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
            else if (resultOperator is FirstResultOperator || resultOperator is SingleResultOperator)
            {
                this.querySpec.Limit = 1;
            }
            else
                throw new NotSupportedException(string.Format("Operator {0} is not supported.", resultOperator.GetType()));
        }

        /// <summary>
        /// Visits the select clause.
        /// </summary>
        /// <param name="selectClause">The select clause.</param>
        /// <param name="queryModel">The query model.</param>
        public override void VisitSelectClause(SelectClause selectClause, QueryModel queryModel)
        {
            this.querySpec.Projection = ProjectionBuilder.Build(this.mongoContext.Configuration.IMappingStore, selectClause.Selector);
        }

        /// <summary>
        /// Visits the where clause.
        /// </summary>
        /// <param name="whereClause">The where clause.</param>
        /// <param name="queryModel">The query model.</param>
        /// <param name="index">The index.</param>
        public override void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index)
        {
            var query = QueryDocumentBuilder.BuildFrom(this.mongoContext, whereClause.Predicate);
            query.CopyTo(this.querySpec.Conditions);            
        }

        #endregion
    }
}