using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;
using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Mapping;

using NUnit.Framework;
using MongoDB.Framework.Proxy.Castle;

namespace MongoDB.Framework
{
    public abstract class TestCase
    {
        protected IMongoSessionFactory mongoSessionFactory;

        protected virtual string DatabaseName
        {
            get { return this.GetType().Name.ToLower(); }
        }

        protected virtual IMongoFactory MongoFactory
        {
            get { return new DefaultMongoFactory(); }
        }

        protected abstract IMappingStore MappingStore { get; }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            mongoSessionFactory = new MongoSessionFactory(this.DatabaseName, this.MappingStore, this.MongoFactory, new CastleProxyGenerator());
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            using (var session = mongoSessionFactory.OpenMongoSession())
                session.Database.MetaData.DropDatabase();
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