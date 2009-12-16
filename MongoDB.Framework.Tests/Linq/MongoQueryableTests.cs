using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;

using NUnit.Framework;

namespace MongoDB.Framework.Linq
{
    [TestFixture]
    public class MongoQueryableTests
    {
        private Mongo mongo;
        private Database database;
        private IMongoCollection partyCollection;
        private EntityMapper entityMapper;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            mongo = new Mongo();
            mongo.Connect();
            database = mongo.getDB("tests");
            partyCollection = database.GetCollection("party");
            var configuration = new MongoConfiguration();
            configuration.AddRootEntityMap(new PartyMap().Instance);
            entityMapper = new EntityMapper(configuration);

            var person1 = new Person()
            {
                Name = "Bob McBob",
                BirthDate = new DateTime(1900, 1, 1),
                PhoneNumber = new PhoneNumber() { AreaCode = "123", Prefix = "456", Number = "7890" },
                ExtendedProperties = new Dictionary<string, object>
                {
                    { "not-mapped", true }
                }
            };

            var person2 = new Person()
            {
                Name = "Jane McJane",
                BirthDate = new DateTime(2000, 2, 2),
                PhoneNumber = new PhoneNumber() { AreaCode = "111", Prefix = "222", Number = "3333" }
            };

            var organization = new Organization()
            {
                Name = "The Muffler Show",
                EmployeeCount = 23,
                PhoneNumber = new PhoneNumber() { AreaCode = "111", Prefix = "654", Number = "3210" }
            };

            var document1 = entityMapper.MapEntityToDocument(person1);
            var document2 = entityMapper.MapEntityToDocument(person2);
            var document3 = entityMapper.MapEntityToDocument(organization);
            partyCollection.Insert(new[] { document1, document2, document3 });
        }

        [Test]
        public void Test_root_query()
        {
            var parties = (from p in new MongoQueryable<Party>(database, entityMapper)
                          where p.PhoneNumber.AreaCode == "111"
                          select p).ToList();

            Assert.AreEqual(2, parties.Count());
        }

        [Test]
        public void Test_discriminated_query()
        {
            var parties = (from p in new MongoQueryable<Organization>(database, entityMapper)
                           where p.PhoneNumber.AreaCode == "111"
                           select p).ToList();

            Assert.AreEqual(1, parties.Count());
        }
        
        [Test]
        public void Test_combinated_query()
        {
            var parties = (from p in new MongoQueryable<Organization>(database, entityMapper)
                           where p.EmployeeCount > 12 && p.EmployeeCount < 24
                           select p).ToList();

            Assert.AreEqual(1, parties.Count());
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            database.SendCommand(new Document().Append("drop", partyCollection.Name));
            mongo.Disconnect();
        }
    }
}