using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MongoDB.Mapper.Queries
{
    [TestFixture]
    public class Skip : QueryTestCase
    {
        [Test]
        public void Should_skip()
        {
            using (var mongoSession = this.OpenMongoSession())
            {
                var entries = mongoSession.Query<BlogEntry>().Skip(1).ToList();

                Assert.AreEqual(2, entries.Count);
            }

        }

    }
}