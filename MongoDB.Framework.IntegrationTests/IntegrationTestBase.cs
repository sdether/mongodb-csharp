using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;
using MongoDB.Framework.Configuration.Fluent.Mapping;
using MongoDB.Framework.Mapping;

using NUnit.Framework;
using MongoDB.Framework.Proxy.Castle;

namespace MongoDB.Framework
{
    public abstract class IntegrationTestBase
    {
        private static IMongoConfiguration mongoConfiguration;
        private static IMongoSessionFactory mongoSessionFactory;

        static IntegrationTestBase()
        {
            var fluentMapModelRegistry = new FluentMapModelRegistry()
                .AddMap(new PartyMap())
                .AddMap(new PhoneNumberMap());

            mongoConfiguration = new MongoConfiguration("tests", fluentMapModelRegistry);
            mongoSessionFactory = mongoConfiguration.CreateMongoSessionFactory();
        }

        protected void SetupEnvironment()
        {
            var mongo = mongoConfiguration.MongoFactory.CreateMongo();
            mongo.Connect();
            Database db = mongo.getDB(mongoConfiguration.DatabaseName);
            string collectionName = mongoSessionFactory.MappingStore.GetClassMapFor<Party>().CollectionName;
            IMongoCollection collection = db.GetCollection(collectionName);

            var party1 = new Document()
                .Append("_id", new Binary(Guid.NewGuid().ToByteArray()) { Subtype = Binary.TypeCode.Uuid })
                .Append("Type", "Person")
                .Append("Name", "Bob McBob")
                .Append("BirthDate", new DateTime(1900, 1, 1))
                .Append("PhoneNumber", new Document()
                    .Append("AreaCode", "123")
                    .Append("Prefix", "456")
                    .Append("Number", "7890"))
                .Append("Aliases", new [] { "Grumpy", "Dopey", "Sleepy" })
                .Append("not-mapped", true);
            var party2 = new Document()
                .Append("_id", new Binary(Guid.NewGuid().ToByteArray()) { Subtype = Binary.TypeCode.Uuid })
                .Append("Type", "Person")
                .Append("Name", "Jane McJane")
                .Append("BirthDate", new DateTime(2000, 2, 2))
                .Append("PhoneNumber", new Document()
                    .Append("AreaCode", "111")
                    .Append("Prefix", "222")
                    .Append("Number", "3333"))
                .Append("not-mapped", true);
            var party3 = new Document()
                .Append("_id", new Binary(Guid.NewGuid().ToByteArray()) { Subtype = Binary.TypeCode.Uuid })
                .Append("Type", "Organization")
                .Append("Name", "The Muffler Shop")
                .Append("EmployeeCount", 23)
                .Append("PhoneNumber", new Document()
                    .Append("AreaCode", "111")
                    .Append("Prefix", "654")
                    .Append("Number", "3210"));

            collection.Insert(new[] { party1, party2, party3 });
            mongo.Disconnect();
        }

        protected IMongoSession CreateMongoSession()
        {
            return mongoSessionFactory.OpenMongoSession();
        }

        protected void TearDownEnvironment()
        {
            var mongo = mongoConfiguration.MongoFactory.CreateMongo();
            mongo.Connect();
            Database db = mongo.getDB(mongoConfiguration.DatabaseName);
            db.MetaData.DropDatabase();
            mongo.Disconnect();
        }

        public class PartyMap : FluentRootClass<Party>
        {
            public PartyMap()
            {
                UseCollection("parties");
                Index().Ascending("Name");

                Id(x => x.Id);

                Map(x => x.Name);
                Map(x => x.PhoneNumber);

                Collection(x => x.AlternatePhoneNumbers);
                Collection(x => x.Aliases);

                DiscriminateSubClassesOnKey<string>("Type")
                    .SubClass<Person>(PartyType.Person.ToString(), m =>
                    {
                        m.Map(x => x.BirthDate);
                    })
                    .SubClass<Organization>(PartyType.Organization.ToString(), m =>
                    {
                        m.Map(x => x.EmployeeCount);
                    });

                ExtendedProperties(x => x.ExtendedProperties);
            }
        }

        public class PhoneNumberMap : FluentNestedClass<PhoneNumber>
        {
            public PhoneNumberMap()
            {
                Map(x => x.AreaCode);
                Map(x => x.Prefix);
                Map(x => x.Number);
            }
        }

        public enum PartyType
        {
            Organization,
            Person
        }

        public class PhoneNumber
        {
            public string AreaCode { get; set; }
            public string Prefix { get; set; }
            public string Number { get; set; }
        }

        public abstract class Party
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public PhoneNumber PhoneNumber { get; set; }

            public IDictionary<string, PhoneNumber> AlternatePhoneNumbers { get; set; }

            public IList<string> Aliases { get; private set; }

            public abstract PartyType Type { get; }

            public IDictionary<string, object> ExtendedProperties { get; set; }

            public Party()
            {
                this.Aliases = new List<string>();
                this.AlternatePhoneNumbers = new Dictionary<string, PhoneNumber>();
                this.ExtendedProperties = new Dictionary<string, object>();
            }
        }

        public class Organization : Party
        {
            public int EmployeeCount { get; set; }

            public override PartyType Type
            {
                get { return PartyType.Organization; }
            }
        }

        public class Person : Party
        {
            public DateTime BirthDate { get; set; }

            public override PartyType Type
            {
                get { return PartyType.Person; }
            }
        }
    }
}