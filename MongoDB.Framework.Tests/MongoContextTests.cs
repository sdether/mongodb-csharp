using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MongoDB.Framework
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
            var parties = (from p in context.Query<Party>()
                           where p.PhoneNumber.AreaCode == "111"
                           select p).ToList();

            Assert.AreEqual(2, parties.Count());
        }

        [Test]
        public void Test_root_entity_query_with_a_nestedClass_condition()
        {
            var parties = (from p in context.Query<Party>()
                           where p.PhoneNumber == new PhoneNumber() { AreaCode = "111", Prefix = "222", Number = "3333" }
                           select p).ToList();

            Assert.AreEqual(1, parties.Count());
        }

        [Test]
        public void Test_discriminated_entity_query()
        {
            var parties = (from p in context.Query<Organization>()
                           where p.PhoneNumber.AreaCode == "111"
                           select p).ToList();

            Assert.AreEqual(1, parties.Count());
        }

        [Test]
        public void Test_combined_query()
        {
            var parties = (from p in context.Query<Organization>()
                           where p.EmployeeCount > 12 && p.EmployeeCount < 24
                           select p).ToList();

            Assert.AreEqual(1, parties.Count());
        }

        [Test]
        public void Test_skip_operator()
        {
            var parties = context.Query<Party>().Skip(1).ToList();

            Assert.AreEqual(2, parties.Count());
        }

        [Test]
        public void Test_take_operator()
        {
            var parties = context.Query<Party>().Take(1).ToList();

            Assert.AreEqual(1, parties.Count());
        }

        [Test]
        public void Test_skip_and_take_operator()
        {
            var parties = context.Query<Party>().Skip(1).Take(2).ToList();

            Assert.AreEqual(2, parties.Count());
        }

        [Test]
        public void Test_count_with_root_entity()
        {
            var partyCount = context.Query<Party>().Count();

            Assert.AreEqual(3, partyCount);
        }

        [Test]
        public void Test_count_with_discriminated_entity()
        {
            var personCount = context.Query<Person>().Count();

            Assert.AreEqual(2, personCount);
        }

        [Test]
        public void Test_first_with_root_entity()
        {
            var party = context.Query<Party>().First();

            Assert.IsNotNull(party);
        }

        [Test]
        public void Test_first_with_discriminated_entity()
        {
            var person = context.Query<Person>().First();

            Assert.IsNotNull(person);
        }

        [Test]
        public void Test_ordering()
        {
            var parties = (from p in context.Query<Party>()
                           orderby p.Name descending
                           select p).ToList();

            Assert.AreEqual("The Muffler Shop", parties[0].Name);
            Assert.AreEqual("Jane McJane", parties[1].Name);
            Assert.AreEqual("Bob McBob", parties[2].Name);
        }

        [Test]
        public void Test_simple_projection()
        {
            var numbers = (from p in context.Query<Person>()
                         select p.PhoneNumber.Number).ToList();

            Assert.AreEqual(2, numbers.Count);
            Assert.Contains("7890", numbers);
            Assert.Contains("3333", numbers);
        }

        [Test]
        public void Test_complex_projection()
        {
            var peopleNumbers = (from p in context.Query<Party>()
                           select new { p.Name, p.PhoneNumber.Number }).ToList();

            Assert.AreEqual(3, peopleNumbers.Count);
        }

        [Test]
        public void Test_updating()
        {
            var party = context.Query<Party>().First(p => p.Name == "Bob McBob");

            party.Name = "Jack";

            context.SubmitChanges();

            using (var context2 = this.CreateContext())
            {
                Assert.IsNotNull(context.Query<Party>().First(p => p.Name == "Jack"));
            }
        }

        [Test]
        public void Test_deleting()
        {
            var party = context.Query<Party>().First(p => p.Name == "Bob McBob");
            context.DeleteOnSubmit(party);

            context.SubmitChanges();

            using (var context2 = this.CreateContext())
            {
                Assert.IsNull(context.Query<Party>().FirstOrDefault(p => p.Name == "Bob McBob"));
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