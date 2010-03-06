using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MongoDB.Mapper.Queries
{
    [TestFixture]
    public class Count : QueryTestCase
    {
        [Test]
        public void Should_count_without_condition()
        {
            using (var mongoSession = this.OpenMongoSession())
            {
                var count = mongoSession.Query<BlogEntry>().Count();

                Assert.AreEqual(3, count);
            }
        }

        [Test]
        public void Should_count_with_condition()
        {
            using (var mongoSession = this.OpenMongoSession())
            {
                var count = mongoSession.Query<BlogEntry>().Where(e => e.Title == "My First Post").Count();

                Assert.AreEqual(1, count);
            }
        }

    }
}