using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace MongoDB.Framework.Queries
{
    [TestFixture]
    public class TopLevelCondition : QueryTestCase
    {

        [Test]
        public void When_rows_exist()
        {
            using(var mongoSession = this.OpenMongoSession())
            {
                var blogs = (from b in mongoSession.Query<Blog>()
                             where b.Name == "Bob McBob's Blog"
                             select b).ToList();

                Assert.AreEqual(1, blogs.Count);
            }

        }

        [Test]
        public void When_no_rows_exist()
        {
            using (var mongoSession = this.OpenMongoSession())
            {
                var blogs = (from b in mongoSession.Query<Blog>()
                             where b.Name == "wgwegwe"
                             select b).ToList();

                Assert.AreEqual(0, blogs.Count);
            }

        }

    }
}
