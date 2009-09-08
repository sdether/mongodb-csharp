﻿using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace MongoDB.Driver
{
	[TestFixture]
	public class TestDatabase
	{
		Mongo mongo = new Mongo();
		
		[Test]
		public void TestFollowReference(){
			Database tests = mongo["tests"];
			Oid id = new Oid("4a7067c30a57000000008ecb");
			DBRef rf = new DBRef("reads", id);
			
			Document target = tests.FollowReference(rf);
			Assert.IsNotNull(target, "FollowReference returned null");
			Assert.IsTrue(target.Contains("j"));
			Assert.AreEqual((double)9980, (double)target["j"]);
		}
		
		[Test]
		public void TestFollowNonReference(){
			Database tests = mongo["tests"];
			Oid id = new Oid("BAD067c30a57000000008ecb");
			DBRef rf = new DBRef("reads", id);
			
			Document target = tests.FollowReference(rf);
			Assert.IsNull(target, "FollowReference returned wasn't null");			
		}
		
		[Test]
		public void TestReferenceNonOid(){
			Database tests = mongo["tests"];
			Collection refs = tests["refs"];
			
			Document doc = new Document().Append("_id",123).Append("msg", "this has a non oid key");
			refs.Insert(doc);
			
			DBRef rf = new DBRef("refs",123);
			
			Document recv = tests.FollowReference(rf);
			
			Assert.IsNotNull(recv);
			Assert.IsTrue(recv.Contains("msg"));
			Assert.AreEqual(recv["_id"], (long)123);
		}
		
		[Test]
		public void TestGetCollectionNames(){
			List<String> names = mongo["tests"].GetCollectionNames();
			Assert.IsNotNull(names,"No collection names returned");
			Assert.IsTrue(names.Count > 0);
			Assert.IsTrue(names.Contains("tests.reads"));
			
		}
		

		[TestFixtureSetUp]
		public void Init(){
			mongo.Connect();
			cleanDB();
		}
		
		[TestFixtureTearDown]
		public void Dispose(){
			mongo.Disconnect();
		}
		
		protected void cleanDB(){
			mongo["tests"]["$cmd"].FindOne(new Document().Append("drop","refs"));

		}		
	}
}
