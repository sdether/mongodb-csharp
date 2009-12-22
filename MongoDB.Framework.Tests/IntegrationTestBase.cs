using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.Configuration;

using NUnit.Framework;
using MongoDB.Driver;
using MongoDB.Framework.Mapping.Fluent;

namespace MongoDB.Framework
{
    public abstract class IntegrationTestBase
    {
        private static MongoContextFactory contextFactory;

        static IntegrationTestBase()
        {
            var mappingStore = new FluentMappingStore()
                .AddMapsFromAssemblyContaining<PartyMap>();

            var configuration = new MongoConfiguration("tests", mappingStore);
            contextFactory = new MongoContextFactory(configuration);
        }

        protected void SetupEnvironment()
        {
            using (var context = this.CreateContext())
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
                    Name = "The Muffler Shop",
                    EmployeeCount = 23,
                    PhoneNumber = new PhoneNumber() { AreaCode = "111", Prefix = "654", Number = "3210" }
                };

                context.InsertOnSubmit(person1);
                context.InsertOnSubmit(person2);
                context.InsertOnSubmit(organization);
                context.SubmitChanges();
            }
        }

        protected MongoContext CreateContext()
        {
            return contextFactory.CreateContext();
        }

        protected void TearDownEnvironment()
        {
            using (var context = this.CreateContext())
            {
                context.Database.SendCommand(new Document().Append("drop", "parties"));
            }
        }
    }
}