using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.DomainModels;

using NUnit.Framework;

namespace MongoDB.Framework.Linq
{
    [TestFixture]
    public class MongoContextTests : IntegrationTestBase
    {
        private MongoContext context;

        [SetUp]
        public void SetUp()
        {
            this.SetupEnvironment();
            this.context = this.CreateContext();
        }

        [Test]
        public void Test_root_entity_query()
        {
            var parties = context.Find<Party>(new Document().Append("PhoneNumber.AreaCode", "111"));

            Assert.AreEqual(2, parties.Count());
        }

        [Test]
        public void Test_root_entity_query_with_a_nestedClass_condition()
        {
            var parties = context.Find<Party>(new Document().Append("PhoneNumber", new Document().Append("AreaCode", "111").Append("Prefix", "222").Append("Number", "3333")));

            Assert.AreEqual(1, parties.Count());
        }

        [Test]
        public void Test_discriminated_entity_query()
        {
            var orgs = context.Find<Organization>(new Document().Append("PhoneNumber.AreaCode", "111"));

            Assert.AreEqual(1, orgs.Count());
        }

        [Test]
        public void Test_combined_query()
        {
            var orgs = context.Find<Organization>(new Document().Append("EmployeeCount", new Document().Append("$gt", 12).Append("$lt", 24)));

            Assert.AreEqual(1, orgs.Count());
        }

        [Test]
        public void Test_skip_operator()
        {
            var parties = context.FindAll<Party>(0, 1);

            Assert.AreEqual(2, parties.Count());
        }

        [Test]
        public void Test_take_operator()
        {
            var parties = context.FindAll<Party>(1, 0);

            Assert.AreEqual(1, parties.Count());
        }

        [Test]
        public void Test_skip_and_take_operator()
        {
            var parties = context.FindAll<Party>(2, 1);

            Assert.AreEqual(2, parties.Count());
        }

        [Test]
        public void Test_find_one_with_root_entity()
        {
            var party = context.FindOne<Party>(null);

            Assert.IsNotNull(party);
        }

        [Test]
        public void Test_find_one_with_discriminated_entity()
        {
            var person = context.FindOne<Person>(null);

            Assert.IsNotNull(person);
        }

        [Test]
        public void Test_ordering()
        {
            var parties = context.FindAll<Party>(new Document().Append("Name", -1)).ToList();

            Assert.AreEqual("The Muffler Shop", parties[0].Name);
            Assert.AreEqual("Jane McJane", parties[1].Name);
            Assert.AreEqual("Bob McBob", parties[2].Name);
        }

        [Test]
        public void Test_deleting()
        {
            var party = context.FindOne<Party>(new Document().Append("Name", "Bob McBob"));
            context.Delete(party);

            using (var context2 = this.CreateContext())
            {
                Assert.IsNull(context2.FindOne<Party>(new Document().Append("Name", "Bob McBob")));
            }
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
            context = null;
            this.TearDownEnvironment();
        }
    }
}