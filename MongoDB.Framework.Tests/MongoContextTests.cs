using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MongoDB.Framework
{
    [TestFixture]
    public class MongoContextTests
    {
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            Domain.SetupEnvironment();
        }

        [Test]
        public void Test_root_query()
        {
            var parties = (from p in Domain.Context.Query<Party>()
                           where p.PhoneNumber.AreaCode == "111"
                           select p).ToList();

            Assert.AreEqual(2, parties.Count());
        }

        [Test]
        public void Test_discriminated_query()
        {
            var parties = (from p in Domain.Context.Query<Organization>()
                           where p.PhoneNumber.AreaCode == "111"
                           select p).ToList();

            Assert.AreEqual(1, parties.Count());
        }

        [Test]
        public void Test_combinated_query()
        {
            var parties = (from p in Domain.Context.Query<Organization>()
                           where p.EmployeeCount > 12 && p.EmployeeCount < 24
                           select p).ToList();

            Assert.AreEqual(1, parties.Count());
        }

        [Test]
        public void Test_skip_operator()
        {
            var parties = Domain.Context.Query<Party>().Skip(1).ToList();

            Assert.AreEqual(2, parties.Count());
        }

        [Test]
        public void Test_take_operator()
        {
            var parties = Domain.Context.Query<Party>().Take(1).ToList();

            Assert.AreEqual(1, parties.Count());
        }

        [Test]
        public void Test_skip_and_take_operator()
        {
            var parties = Domain.Context.Query<Party>().Skip(1).Take(2).ToList();

            Assert.AreEqual(2, parties.Count());
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            Domain.TearDownEnvironment();
        }
    }
}