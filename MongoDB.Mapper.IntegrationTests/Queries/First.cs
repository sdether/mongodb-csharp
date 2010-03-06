using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MongoDB.Mapper.Queries
{
    [TestFixture]
    public class First : QueryTestCase
    {
        [Test]
        public void Should_get_first()
        {
            using (var mongoSession = this.OpenMongoSession())
            {
                var entry = mongoSession.Query<BlogEntry>().First();

                Assert.IsNotNull(entry);
            }

        }

    }
}