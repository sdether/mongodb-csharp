using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MongoDB.Mapper.Queries
{
    [TestFixture]
    public class Take : QueryTestCase
    {
        [Test]
        public void Should_take()
        {
            using (var mongoSession = this.OpenMongoSession())
            {
                var entries = mongoSession.Query<BlogEntry>().Take(1).ToList();

                Assert.AreEqual(1, entries.Count);
            }

        }

    }
}