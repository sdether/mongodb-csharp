using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MongoDB.Framework.Queries.Discriminators
{
    [TestFixture]
    public class Root : DiscriminatedQueryTestCase
    {

        [Test]
        public void Should_fetch()
        {
            using (var mongoSession = this.OpenMongoSession())
            {
                var parties = mongoSession.Query<Party>().ToList();

                Assert.AreEqual(3, parties.Count);
            }
        }

    }
}