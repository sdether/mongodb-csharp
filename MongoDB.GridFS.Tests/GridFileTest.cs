using System;
using System.IO;

using NUnit.Framework;

using MongoDB.Driver;

namespace MongoDB.GridFS
{
    [TestFixture]
    public class GridFileTest{
        Mongo db = MongoFactory.CreateMongo();

        [Test]
        public void TestFileDoesNotExist(){
            GridFile fs = new GridFile(db["tests"]);
            Assert.IsFalse(fs.Exists("non-existent filename"));
        }

        [Test]
        public void TestFileDoes(){
            GridFile fs = new GridFile(db["tests"]);
            fs.Create("exists.txt");
            Assert.IsTrue(fs.Exists("exists.txt"));
        }
        [Test]
        public void TestCopy(){
            GridFile fs = new GridFile(db["tests"], "gfcopy");
            GridFileStream gfs = fs.Create("original.txt");
            gfs.WriteByte(1);
            gfs.Seek(1024 * 256 * 2, SeekOrigin.Begin);
            gfs.WriteByte(2);
            gfs.Close();
            fs.Copy("original.txt", "copy.txt");
            Assert.IsTrue(fs.Exists("original.txt"));
            Assert.IsTrue(fs.Exists("copy.txt"));
            //TODO Assert chunk data is the same too.
        }
        
        [Test]
        public void TestModeCreateNew(){
            Object id;
            string filename = "createnew.txt";
            GridFile gf = new GridFile(db["tests"],"gfcreate");
            using(GridFileStream gfs = gf.Create(filename, FileMode.CreateNew)){
                id = gfs.GridFileInfo.Id;
                TextWriter tw = new StreamWriter(gfs);
                tw.WriteLine("test");
                tw.Close();
            }
            Assert.AreEqual(1, CountChunks("gfcreate", id));
        }
        
        [TestFixtureSetUp]
        public void Init(){
            db.Connect();
            CleanDB(); //Run here instead of at the end so that the db can be examined after a run.
        }
        
        [TestFixtureTearDown]
        public void Dispose(){
            db.Disconnect();
        }
        
        protected void CleanDB(){
            //Any collections that we might want to delete before the tests run should be done here.
            DropGridFileSystem("gfcopy");
            DropGridFileSystem("gfcreate");
            DropGridFileSystem("fs");
        }
        
        protected void DropGridFileSystem(string filesystem){
            try{
                db["tests"].MetaData.DropCollection(filesystem + ".files");
                db["tests"].MetaData.DropCollection(filesystem + ".chunks");
            }catch(MongoCommandException){}//if it fails it is because the collection isn't there to start with.
            
        }

        protected long CountChunks(string filesystem, Object fileid){
            return db["tests"][filesystem + ".chunks"].Count(new Document().Append("files_id", fileid));
        }        
        
    }
}
