using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.Configuration;
using MongoDB.Framework.Configuration.Mapping;

using NUnit.Framework;
using MongoDB.Driver;

namespace MongoDB.Framework
{
    public abstract class TestCase
    {
        protected IMongoContextFactory contextFactory;

        protected virtual string DatabaseName
        {
            get { return "tests"; }
        }

        protected abstract IMapProvider MapProvider { get; }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            var mappingStore = new MappingStore(this.MapProvider);
            var configuration = new MongoConfiguration(this.DatabaseName, mappingStore);
            contextFactory = new MongoContextFactory(configuration);
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            var mongo = contextFactory.Configuration.MongoFactory.CreateMongo();
            mongo.Connect();
            Database db = mongo.getDB(contextFactory.Configuration.DatabaseName);
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

        protected virtual IMongoContext CreateContext()
        {
            var context = this.contextFactory.CreateContext();
            return context;
        }
    }
}