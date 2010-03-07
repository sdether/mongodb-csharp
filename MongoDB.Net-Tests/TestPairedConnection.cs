using System;
using System.Net.Sockets;

using NUnit.Framework;

namespace MongoDB.Driver
{
    [TestFixture]
    public class TestPairedConnection
    {
        private int DefaultMasterPort = MongoFactory.Port + 1;
        private int DefaultSlavePort = MongoFactory.Port + 2;
        [Test]
        public void TestConnectingToNonPaired(){
            PairedConnection pc = new PairedConnection(MongoFactory.Host, MongoFactory.Port, MongoFactory.Host, DefaultSlavePort);
            pc.Open();
            Assert.AreEqual(ConnectionState.Opened, pc.State);
            Assert.IsFalse(pc.Paired);
        }
        
        [Test]
        public void TestConnectingToMasterReplica(){
            PairedConnection pc = new PairedConnection(MongoFactory.Host, DefaultMasterPort, MongoFactory.Host, DefaultSlavePort);
            pc.Open();
            Assert.AreEqual(ConnectionState.Opened, pc.State);
            Assert.IsTrue(pc.Paired);
            Assert.IsTrue(pc.ConnectedToMaster);
        }

        [Test]
        public void TestConnectingToSlaveOk(){
            PairedConnection pc = new PairedConnection(MongoFactory.Host, DefaultSlavePort, MongoFactory.Host, DefaultMasterPort, true);
            pc.Open();
            Assert.AreEqual(ConnectionState.Opened, pc.State);
            Assert.IsTrue(pc.Paired);
            Assert.IsFalse(pc.ConnectedToMaster);
        }

        [Test]
        public void TestConnectingToSlaveNotOk(){
            PairedConnection pc = new PairedConnection(MongoFactory.Host, DefaultSlavePort, MongoFactory.Host, DefaultMasterPort, false);
            pc.Open();
            Assert.AreEqual(ConnectionState.Opened, pc.State);
            Assert.IsTrue(pc.Paired);
            Assert.IsTrue(pc.ConnectedToMaster);
        }

        [Test]
        public void TestConnectingToDownMaster(){
            int badport = 33333;
            PairedConnection pc = new PairedConnection(MongoFactory.Host, badport, MongoFactory.Host, DefaultMasterPort, false);
            pc.Open();
            Assert.AreEqual(ConnectionState.Opened, pc.State);
            Assert.IsTrue(pc.Paired);
            Assert.IsTrue(pc.ConnectedToMaster);
        }

        [Test]
        public void TestConnectingToDownBothThrowsException(){
            /*
             * Be careful with this test.  The recursion is set in the open to fail out of the 
             * SocketException catches.  If the PairedConnection.Open method is changed to
             * screw that up it will cause an infinite loop.
             */
            int badport = 33333;
            PairedConnection pc = new PairedConnection(MongoFactory.Host, badport, MongoFactory.Host, badport);
            bool thrown = false;
            try{
                pc.Open();    
            }catch(SocketException){
                thrown = true;
            }
            Assert.IsTrue(thrown, "SocketException should have been thrown");
        }
        
        [Test]
        public void TestConnectingToSlaveUsingBadMasterInfoThrowsException(){
            int badport = 33333;
            PairedConnection pc = new PairedConnection(MongoFactory.Host, DefaultSlavePort, MongoFactory.Host, badport, false);
            bool thrown = false;
            try{
                pc.Open();
            }catch(SocketException){
                thrown = true;
            }
            Assert.IsTrue(thrown, "SocketException should have been thrown");
        }
    }
}
