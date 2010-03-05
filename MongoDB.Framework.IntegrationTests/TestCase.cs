using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;
using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Mapping;

using NUnit.Framework;

namespace MongoDB.Framework
{
    public abstract class TestCase
    {
        protected IMongoConfiguration mongoConfiguration;
        protected IMongoSessionFactory mongoSessionFactory;

        protected virtual string DatabaseName
        {
            get { return this.GetType().Name.ToLower(); }
        }

        protected abstract IMapModelRegistry MapModelRegistry { get; }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            mongoConfiguration = new MongoConfiguration(this.DatabaseName) { MappingStore = this.MapModelRegistry.BuildMappingStore() };
            mongoSessionFactory = mongoConfiguration.CreateMongoSessionFactory();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            var mongo = mongoConfiguration.MongoFactory.CreateMongo();
            mongo.Connect();
            Database db = mongo.getDB(mongoConfiguration.DatabaseName);
            db.MetaData.DropDatabase();
            mongo.Disconnect();
        }

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
        }

        [TearDown]
        public void TearDown()
        {
            AfterTest();
        }

        protected virtual void BeforeTest()
        { }

        protected virtual void AfterTest()
        { }

        protected virtual IMongoSession OpenMongoSession()
        {
            return this.mongoSessionFactory.OpenMongoSession();
        }
    }
}