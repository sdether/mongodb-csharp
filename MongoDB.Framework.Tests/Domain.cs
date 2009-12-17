using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.Configuration.Fluent;
using MongoDB.Framework.Configuration;
using MongoDB.Driver;

namespace MongoDB.Framework
{
    public static class Domain
    {
        public static MongoContextFactory ContextFactory { get; private set; }

        public static MongoContext Context { get; private set; }

        static Domain()
        {
            var configuration = new MongoConfiguration()
            {
                DatabaseName = "tests"
            };
            configuration.AddRootEntityMap(new PartyMap().Instance);
            ContextFactory = new MongoContextFactory(configuration);
        }

        public static void SetupEnvironment()
        {
            using (var context = ContextFactory.OpenContext())
            {
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

                context.InsertOnSubmit(person1);
                context.InsertOnSubmit(person2);
                context.InsertOnSubmit(organization);
                context.SubmitChanges();
            }
        }

        public static void TearDownEnvironment()
        {
            using (var context = ContextFactory.OpenContext())
            {
                context.Database.SendCommand(new Document().Append("drop", "party"));
            }
        }
    }

    public class PartyMap : FluentRootEntityMap<Party>
    {
        public PartyMap()
        {
            WithCollectionName("party");
            Index("phone_area").Ascending("PhoneNumber.area");

            Id(x => x.Id);
            Map("Name");

            Entity<PhoneNumber>(x => x.PhoneNumber, m =>
            {
                m.Map(x => x.AreaCode, "area");
                m.Map(x => x.Prefix, "pfx");
                m.Map(x => x.Number, "num");
            });

            DiscriminateBy<string>("Type")
                .Entity<Organization>(PartyType.Organization.ToString(), m =>
                {
                    m.Map(x => x.EmployeeCount);
                })
                .Entity<Person>(PartyType.Person.ToString(), m =>
                {
                    m.Map(x => x.BirthDate);
                });

            UseExtendedProperties(x => x.ExtendedProperties);
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
        public string Id { get; set; }

        public string Name { get; set; }

        public PhoneNumber PhoneNumber { get; set; }

        public abstract PartyType Type { get; }

        public IDictionary<string, object> ExtendedProperties { get; set; }
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