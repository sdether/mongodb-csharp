using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;
using MongoDB.Framework.Configuration.Fluent.Mapping;
using MongoDB.Framework.DomainModels;
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
                .AddMapsFromAssemblyContaining<PartyMap>();

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
                .Append("Type", "Person")
                .Append("Name", "Jane McJane")
                .Append("BirthDate", new DateTime(2000, 2, 2))
                .Append("PhoneNumber", new Document()
                    .Append("AreaCode", "111")
                    .Append("Prefix", "222")
                    .Append("Number", "3333"))
                .Append("not-mapped", true);
            var party3 = new Document()
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
            string collectionName = mongoSessionFactory.MappingStore.GetClassMapFor<Party>().CollectionName;
            db.SendCommand(new Document().Append("drop", collectionName));
            mongo.Disconnect();
        }
    }
}