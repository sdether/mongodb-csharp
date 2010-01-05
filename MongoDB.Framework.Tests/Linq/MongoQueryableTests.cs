using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.DomainModels;

using NUnit.Framework;

namespace MongoDB.Framework.Linq
{
    [TestFixture]
    public class MongoQueryableTests : IntegrationTestBase
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
            var parties = (from p in mongoSession.Query<Party>()
                           where p.PhoneNumber.AreaCode == "111"
                           select p).ToList();

            Assert.AreEqual(2, parties.Count());
        }

        [Test]
        public void Test_root_entity_query_with_a_nestedClass_condition()
        {
            var parties = (from p in mongoSession.Query<Party>()
                           where p.PhoneNumber == new PhoneNumber() { AreaCode = "111", Prefix = "222", Number = "3333" }
                           select p).ToList();

            Assert.AreEqual(1, parties.Count());
        }

        [Test]
        public void Test_discriminated_entity_query()
        {
            var parties = (from p in mongoSession.Query<Organization>()
                           where p.PhoneNumber.AreaCode == "111"
                           select p).ToList();

            Assert.AreEqual(1, parties.Count());
        }

        [Test]
        public void Test_combined_query()
        {
            var parties = (from p in mongoSession.Query<Organization>()
                           where p.EmployeeCount > 12 && p.EmployeeCount < 24
                           select p).ToList();

            Assert.AreEqual(1, parties.Count());
        }

        [Test]
        public void Test_skip_operator()
        {
            var parties = mongoSession.Query<Party>().Skip(1).ToList();

            Assert.AreEqual(2, parties.Count());
        }

        [Test]
        public void Test_take_operator()
        {
            var parties = mongoSession.Query<Party>().Take(1).ToList();

            Assert.AreEqual(1, parties.Count());
        }

        [Test]
        public void Test_skip_and_take_operator()
        {
            var parties = mongoSession.Query<Party>().Skip(1).Take(2).ToList();

            Assert.AreEqual(2, parties.Count());
        }

        [Test]
        public void Test_count_with_root_entity()
        {
            var partyCount = mongoSession.Query<Party>().Count();

            Assert.AreEqual(3, partyCount);
        }

        [Test]
        public void Test_count_with_discriminated_entity()
        {
            var personCount = mongoSession.Query<Person>().Count();

            Assert.AreEqual(2, personCount);
        }

        [Test]
        public void Test_first_with_root_entity()
        {
            var party = mongoSession.Query<Party>().First();

            Assert.IsNotNull(party);
        }

        [Test]
        public void Test_first_with_discriminated_entity()
        {
            var person = mongoSession.Query<Person>().First();

            Assert.IsNotNull(person);
        }

        [Test]
        public void Test_ordering()
        {
            var parties = (from p in mongoSession.Query<Party>()
                           orderby p.Name descending
                           select p).ToList();

            Assert.AreEqual("The Muffler Shop", parties[0].Name);
            Assert.AreEqual("Jane McJane", parties[1].Name);
            Assert.AreEqual("Bob McBob", parties[2].Name);
        }

        [Test]
        public void Test_simple_projection()
        {
            var numbers = (from p in mongoSession.Query<Person>()
                         select p.PhoneNumber.Number).ToList();

            Assert.AreEqual(2, numbers.Count);
            Assert.Contains("7890", numbers);
            Assert.Contains("3333", numbers);
        }

        [Test]
        public void Test_complex_projection()
        {
            var peopleNumbers = (from p in mongoSession.Query<Party>()
                           select new { p.Name, p.PhoneNumber.Number }).ToList();

            Assert.AreEqual(3, peopleNumbers.Count);
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