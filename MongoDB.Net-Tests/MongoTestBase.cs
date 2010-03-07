using System;
using NUnit.Framework;

namespace MongoDB.Driver
{

    public abstract class MongoTestBase
    {
        public Mongo Mongo{get;set;}
        public Database DB{
            get{
                return this.Mongo["tests"];
            }
        }
        
        /// <summary>
        /// Comma separated list of collections to clean at startup.
        /// </summary>
        public abstract string TestCollections{get;}
        

        [TestFixtureSetUp]
        public virtual void Init(){
            this.Mongo = MongoFactory.CreateMongo();
            this.Mongo.Connect();
            cleanDB();
        }
        
        
        [TestFixtureTearDown]
        public virtual void Dispose(){
            this.Mongo.Disconnect();
        }
        
        protected void cleanDB(){
            foreach(string col in this.TestCollections.Split(',')){
                DB["$cmd"].FindOne(new Document(){{"drop", col.Trim()}});
                Console.WriteLine("Dropping " + col);
            }
        }
    }
}