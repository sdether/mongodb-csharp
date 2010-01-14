using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MongoDB.Framework.Queries
{
    [TestFixture]
    public class SkipAndTake : QueryTestCase
    {
        [Test]
        public void Should_skip_and_take()
        {
            using (var mongoSession = this.OpenMongoSession())
            {
                var entries = mongoSession.Query<BlogEntry>().Skip(1).Take(2).ToList();

                Assert.AreEqual(2, entries.Count);
            }

        }
    }
}