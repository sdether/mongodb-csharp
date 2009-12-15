using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace MongoDB.Framework.Visitors
{
    public class MemberAccessMemberInfoVisitor : ExpressionVisitor
    {
        #region Private Fields

        private Stack<MemberInfo> members = new Stack<MemberInfo>();

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the member.
        /// </summary>
        /// <value>The member.</value>
        public IEnumerable<MemberInfo> Members
        {
            get { return this.members; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Overriden. Overrides all MemberAccess to build a path string.
        /// </summary>
        /// <param name="memberExpression"></param>
        /// <returns></returns>
        protected override Expression VisitMemberAccess(MemberExpression memberExpression)
        {
            this.members.Push(memberExpression.Member);
            return base.VisitMemberAccess(memberExpression);
        }

        #endregion
    }
}