using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;
using MongoDB.Framework.Proxy.Castle;
using MongoDB.Framework.Mapping;

using NUnit.Framework;

namespace MongoDB.Framework
{
    public abstract class TestCase
    {
        protected IMongoSessionFactory mongoSessionFactory;

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
            configuration.ProxyGenerator = new CastleProxyGenerator();
            mongoSessionFactory = configuration.CreateMongoSessionFactory();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            var mongo = mongoSessionFactory.Configuration.MongoFactory.CreateMongo();
            mongo.Connect();
            Database db = mongo.getDB(mongoSessionFactory.Configuration.DatabaseName);
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