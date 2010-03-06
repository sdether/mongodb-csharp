using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Mapper.Configuration;
using MongoDB.Mapper.Configuration.Mapping;
using MongoDB.Mapper.Mapping;

using NUnit.Framework;
using MongoDB.Mapper.Proxy.Castle;

namespace MongoDB.Mapper
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

        protected virtual IMappingStore MappingStore
        {
            get { return new AutoMappingStore(); }
        }

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