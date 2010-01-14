using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MongoDB.Framework.Queries
{
    [TestFixture]
    public class Ordering : QueryTestCase
    {
        [Test]
        public void Should_order_ascending()
        {
            using (var mongoSession = this.OpenMongoSession())
            {
                var entries = (from e in mongoSession.Query<BlogEntry>()
                               orderby e.Title
                               select e).ToList();

                Assert.AreEqual("My First Post", entries[0].Title);
                Assert.AreEqual("My Second Post", entries[1].Title);
                Assert.AreEqual("Third Post", entries[2].Title);
            }
        }

        [Test]
        public void Should_order_descending()
        {
            using (var mongoSession = this.OpenMongoSession())
            {
                var entries = (from e in mongoSession.Query<BlogEntry>()
                               orderby e.Title descending
                               select e).ToList();

                Assert.AreEqual("Third Post", entries[0].Title);
                Assert.AreEqual("My Second Post", entries[1].Title);
                Assert.AreEqual("My First Post", entries[2].Title);
            }
        }

    }
}