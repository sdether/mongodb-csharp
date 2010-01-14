using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MongoDB.Framework.Queries
{
    [TestFixture]
    public class Projection : QueryTestCase
    {
        [Test]
        public void simple()
        {
            using (var mongoSession = this.OpenMongoSession())
            {
                var titles = (from e in mongoSession.Query<BlogEntry>()
                               select e.Title).ToList();

                Assert.AreEqual(3, titles.Count);
                Assert.AreEqual("My First Post", titles[0]);
                Assert.AreEqual("My Second Post", titles[1]);
                Assert.AreEqual("Third Post", titles[2]);
            }
        }

        [Test]
        public void complex()
        {
            using (var mongoSession = this.OpenMongoSession())
            {
                var summary = (from e in mongoSession.Query<BlogEntry>()
                               select new { Title = e.Title, NumComments = e.PostDate }).ToList();

                Assert.AreEqual(3, summary.Count);
                Assert.AreEqual("My First Post", summary[0].Title);
                Assert.AreEqual("My Second Post", summary[1].Title);
                Assert.AreEqual("Third Post", summary[2].Title);
            }
        }
    }
}