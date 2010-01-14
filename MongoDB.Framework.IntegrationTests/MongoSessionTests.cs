using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;

using NUnit.Framework;

namespace MongoDB.Framework.Linq
{
    [TestFixture]
    public class MongoSessionTests : IntegrationTestBase
    {
        private IMongoSession mongoSession;

        [SetUp]
        public void SetUp()
        {
            this.SetupEnvironment();
            this.mongoSession = this.CreateMongoSession();
        }

        [Test]
        public void Test_root_entity_query()
        {
            var parties = mongoSession.Find<Party>(new Document().Append("PhoneNumber.AreaCode", "111"));

            Assert.AreEqual(2, parties.Count());
        }

        [Test]
        public void Test_root_entity_query_with_a_nestedClass_condition()
        {
            var parties = mongoSession.Find<Party>(new Document().Append("PhoneNumber", new Document().Append("AreaCode", "111").Append("Prefix", "222").Append("Number", "3333")));

            Assert.AreEqual(1, parties.Count());
        }

        [Test]
        public void Test_discriminated_entity_query()
        {
            var orgs = mongoSession.Find<Organization>(new Document().Append("PhoneNumber.AreaCode", "111"));

            Assert.AreEqual(1, orgs.Count());
        }

        [Test]
        public void Test_combined_query()
        {
            var orgs = mongoSession.Find<Organization>(new Document().Append("EmployeeCount", new Document().Append("$gt", 12).Append("$lt", 24)));

            Assert.AreEqual(1, orgs.Count());
        }

        [Test]
        public void Test_skip_operator()
        {
            var parties = mongoSession.FindAll<Party>(0, 1);

            Assert.AreEqual(2, parties.Count());
        }

        [Test]
        public void Test_take_operator()
        {
            var parties = mongoSession.FindAll<Party>(1, 0);

            Assert.AreEqual(1, parties.Count());
        }

        [Test]
        public void Test_skip_and_take_operator()
        {
            var parties = mongoSession.FindAll<Party>(2, 1);

            Assert.AreEqual(2, parties.Count());
        }

        [Test]
        public void Test_find_one_with_root_entity()
        {
            var party = mongoSession.FindOne<Party>(null);

            Assert.IsNotNull(party);
        }

        [Test]
        public void Test_find_one_with_discriminated_entity()
        {
            var person = mongoSession.FindOne<Person>(null);

            Assert.IsNotNull(person);
        }

        [Test]
        public void Test_ordering()
        {
            var parties = mongoSession.FindAll<Party>(new Document().Append("Name", -1)).ToList();

            Assert.AreEqual("The Muffler Shop", parties[0].Name);
            Assert.AreEqual("Jane McJane", parties[1].Name);
            Assert.AreEqual("Bob McBob", parties[2].Name);
        }

        [Test]
        public void Test_deleting()
        {
            var party = mongoSession.FindOne<Party>(new Document().Append("Name", "Bob McBob"));
            mongoSession.DeleteOnSubmit(party);
            mongoSession.SubmitChanges();

            using (var mongoSession2 = this.CreateMongoSession())
            {
                Assert.IsNull(mongoSession2.FindOne<Party>(new Document().Append("Name", "Bob McBob")));
            }
        }

        [TearDown]
        public void TearDown()
        {
            mongoSession.Dispose();
            mongoSession = null;
            this.TearDownEnvironment();
        }
    }
}