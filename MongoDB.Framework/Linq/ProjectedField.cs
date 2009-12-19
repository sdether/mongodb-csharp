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

namespace MongoDB.Framework.Linq
{
    public class ProjectedField
    {
        public string DocumentKey
        {
            get { return string.Join(".", this.MemberMapPath.Select(mm => mm.DocumentKey).ToArray()); }
        }

        public IEnumerable<MemberMap> MemberMapPath { get; private set; }

        public ProjectedField(IEnumerable<MemberMap> memberMapPath)
        {
            if (memberMapPath == null)
                throw new ArgumentNullException("memberMapPath");

            this.MemberMapPath = memberMapPath;
        }
    }
}
