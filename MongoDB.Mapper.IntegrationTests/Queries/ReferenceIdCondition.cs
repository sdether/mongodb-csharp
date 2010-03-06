using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace MongoDB.Mapper.Queries
{
    [TestFixture]
    public class ReferenceIdCondition : QueryTestCase
    {

        [Test]
        public void When_rows_exist()
        {
            using(var mongoSession = this.OpenMongoSession())
            {
                var blog = mongoSession.Query<Blog>().Single();
                var entries = (from e in mongoSession.Query<BlogEntry>()
                             where e.Blog.Id == blog.Id
                             select e).ToList();

                Assert.AreEqual(3, entries.Count);
            }

        }
    }
}
