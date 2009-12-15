using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using Remotion.Data.Linq;
using Remotion.Data.Linq.Clauses;

namespace MongoDB.Framework.Linq.Visitors
{
    public class MongoQueryModelVisitor : QueryModelVisitorBase
    {
        #region Public Static Methods

        /// <summary>
        /// Translates the QueryModel to a MongoQuerySpec.
        /// </summary>
        /// <param name="queryModel">The query model.</param>
        /// <param name="entityMapper">The entity mapper.</param>
        /// <returns></returns>
        public static MongoQuerySpec TranslateToMongoQuerySpec(QueryModel queryModel, EntityMapper entityMapper)
        {
            if (queryModel == null)
                throw new ArgumentNullException("queryModel");
            if (entityMapper == null)
                throw new ArgumentNullException("entityMapper");

            var visitor = new MongoQueryModelVisitor(entityMapper);
            visitor.VisitQueryModel(queryModel);
            return visitor.querySpec;
        }

        #endregion

        #region Private Fields

        private EntityMapper entityMapper;
        private MongoQuerySpec querySpec;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoQueryModelVisitor"/> class.
        /// </summary>
        /// <param name="entityMapper">The entity mapper.</param>
        private MongoQueryModelVisitor(EntityMapper entityMapper)
        {
            this.entityMapper = entityMapper;
            this.querySpec = new MongoQuerySpec();
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
        }

        /// <summary>
        /// Visits the select clause.
        /// </summary>
        /// <param name="selectClause">The select clause.</param>
        /// <param name="queryModel">The query model.</param>
        public override void VisitSelectClause(SelectClause selectClause, QueryModel queryModel)
        {
            //MongoSelectClauseExpressionTreeVisitor.PopulateFields(this.querySpec, this.entityMapper, selectClause.Selector);
            base.VisitSelectClause(selectClause, queryModel);
        }

        /// <summary>
        /// Visits the where clause.
        /// </summary>
        /// <param name="whereClause">The where clause.</param>
        /// <param name="queryModel">The query model.</param>
        /// <param name="index">The index.</param>
        public override void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index)
        {
            MongoWhereClauseExpressionTreeVisitor.PopulateSpec(this.querySpec, this.entityMapper.Configuration, whereClause.Predicate);

            base.VisitWhereClause(whereClause, queryModel, index);
        }

        #endregion
    }
}