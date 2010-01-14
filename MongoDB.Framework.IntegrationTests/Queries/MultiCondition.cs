using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace MongoDB.Framework.Queries
{
    [TestFixture]
    public class MultiCondition : QueryTestCase
    {

        [Test]
        public void When_rows_exist()
        {
            using(var mongoSession = this.OpenMongoSession())
            {
                var entries = (from e in mongoSession.Query<BlogEntry>()
                             where e.PostDate >= new DateTime(2008, 12, 31, 0, 0, 0, DateTimeKind.Utc)
                                && e.PostDate <= new DateTime(2009, 1, 2, 0, 0, 0, DateTimeKind.Utc)
                             select e).ToList();

                Assert.AreEqual(2, entries.Count);
            }
        }

    }
}
