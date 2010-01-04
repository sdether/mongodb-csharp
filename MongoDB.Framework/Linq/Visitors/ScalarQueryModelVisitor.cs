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
using MongoDB.Framework.Configuration.Mapping;

namespace MongoDB.Framework.Linq.Visitors
{
    public class ScalarQueryModelVisitor : QueryModelVisitorBase
    {
        #region Private Fields

        private IMongoContext mongoContext;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether this instance is count.
        /// </summary>
        /// <value><c>true</c> if this instance is count; otherwise, <c>false</c>.</value>
        public bool IsCount { get; private set; }

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <value>The query.</value>
        public Document Query { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoQueryModelVisitor"/> class.
        /// </summary>
        /// <param name="mongoContext">The mongo context.</param>
        public ScalarQueryModelVisitor(IMongoContext mongoContext)
        {
            this.mongoContext = mongoContext;
            this.Query = new Document();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Visits the query model.
        /// </summary>
        /// <param name="queryModel">The query model.</param>
        public override void VisitQueryModel(QueryModel queryModel)
        {
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
            if (resultOperator is CountResultOperator)
            {
                this.IsCount = true;
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
            this.Query = QueryDocumentBuilder.BuildFrom(this.mongoContext, whereClause.Predicate);
        }

        #endregion
    }
}