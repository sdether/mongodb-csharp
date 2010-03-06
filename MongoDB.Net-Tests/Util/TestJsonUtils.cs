
using System;
using NUnit.Framework;

namespace MongoDB.Driver.Util
{


    [TestFixture]
    public class TestJsonUtils
    {
        [Test]
        public void TestNothingToEscape(){
            string str = "There is nothing here to escape";
            Assert.AreEqual(str, JsonUtils.Escape(str));
        }
        
        [Test]
        public void TestAsciiToEscape(){
            string str = "\b \f \n \r \t \v \' \" \\";
            string expected = @"\b \f \n \r \t \v \' \"" \\";
            Assert.AreEqual(expected, JsonUtils.Escape(str));
        }
        
        [Test]
        public void TestPrintableUnicode(){
            string str = "pi (π)";
            Assert.AreEqual(str, JsonUtils.Escape(str));
        }
        
        [Test]
        public void TestNonPrintableUnicode(){
            string str = "\u0007 bell";
            string expected = @"\u0007 bell";
            Assert.AreEqual(expected, JsonUtils.Escape(str));
        }
        
        [Test]
        public void TestSerializeDocWithSingleNullField() {
            var doc = new Document().Append("foo", null);
            Assert.AreEqual(@"{ ""foo"": null }", JsonUtils.Serialize(doc));
        }

        [Test]
        public void TestSerializeDocWithSingleTrueField() {
            var doc = new Document().Append("foo", true);
            Assert.AreEqual(@"{ ""foo"": true }", JsonUtils.Serialize(doc));
        }

        [Test]
        public void TestSerializeDocWithSingleFalseField() {
            var doc = new Document().Append("foo", false);
            Assert.AreEqual(@"{ ""foo"": false }", JsonUtils.Serialize(doc));
        }

        [Test]
        public void TestSerializeDocWithSingleStringField() {
            var doc = new Document().Append("foo", "bar");
            Assert.AreEqual(@"{ ""foo"": ""bar"" }", JsonUtils.Serialize(doc));
        }
        
        [Test]
        public void TestSerializeDocWithLowUnicodeValues(){
            var doc = new Document(){{"foo", "\u0007 bell"}};
            Assert.AreEqual(@"{ ""foo"": ""\u0007 bell"" }", JsonUtils.Serialize(doc));
        }
        [Test]
        public void TestSerializeDocWithHighUnicodeValues(){
            var doc = new Document(){{"foo", "pi (π)"}};
            Assert.AreEqual(@"{ ""foo"": ""pi (π)"" }", JsonUtils.Serialize(doc));
        }        

        [Test]
        public void TestSerializeDocWithSingleIntField() {
            var doc = new Document().Append("foo", 10);
            Assert.AreEqual(@"{ ""foo"": 10 }", JsonUtils.Serialize(doc));
        }

        [Test]
        public void TestSerializeDocWithSingleDoubleField() {
            var doc = new Document().Append("foo", 10.1);
            Assert.AreEqual(@"{ ""foo"": 10.1 }", JsonUtils.Serialize(doc));
        }

        [Test]
        public void TestSerializeDocWithSingleDateTimeField() {
            var doc = new Document().Append("foo", DateTime.Parse("2009-10-10T07:00:00.0000000Z"));
            Assert.AreEqual(@"{ ""foo"": ""2009-10-10T07:00:00.0000000Z"" }", JsonUtils.Serialize(doc));
        }

        [Test]
        public void TestSerializeDocWithSingleOidField() {
            var doc = new Document().Append("foo", new Oid("4ac7ee91f693066f1c96b649"));
            Assert.AreEqual(@"{ ""foo"": ""4ac7ee91f693066f1c96b649"" }", JsonUtils.Serialize(doc));
        }
        [Test]
        public void TestSerializeDocWithMultipleFields() {
            var doc = new Document().Append("foo", "bar").Append("baz", 42);
            Assert.AreEqual(@"{ ""foo"": ""bar"", ""baz"": 42 }", JsonUtils.Serialize(doc));
        }

        [Test]
        public void TestSerializeDocWithSubDocField() {
            var doc = new Document().Append("foo", "bar").Append("baz", new Document().Append("a", 1));
            Assert.AreEqual(@"{ ""foo"": ""bar"", ""baz"": { ""a"": 1 } }", JsonUtils.Serialize(doc));
        }
        [Test]
        public void TestSerializeDocWithArrayOfInts() {
            var doc = new Document().Append("foo", new[] {1,2,3,4});
            Assert.AreEqual(@"{ ""foo"": [ 1, 2, 3, 4 ] }", JsonUtils.Serialize(doc));
        }

        [Test]
        public void TestSerializeDocWithArrayOfDocs() {
            var doc = new Document().Append("foo", new[] {
                new Document().Append("a", 1),
                new Document().Append("b", 2),
                new Document().Append("c", 3),
            });
            Assert.AreEqual(@"{ ""foo"": [ { ""a"": 1 }, { ""b"": 2 }, { ""c"": 3 } ] }", JsonUtils.Serialize(doc));
        }
        
        [Test]
        public void TestSerializeDocWithBinary(){
            var doc = new Document(){{"b", new Binary(){Bytes = new byte[]{0,1,2,3,4}, 
                                                            Subtype = Binary.TypeCode.General}}};
            Assert.AreEqual(@"{ ""b"": { ""$binary"": ""AAECAwQ="", ""$type"" : 2 } }", 
                            JsonUtils.Serialize(doc));
        }
        
        [Test]
        public void TestSerializeDocWithGUID(){
            var doc = new Document(){{"guid", new Guid("936da01f-9abd-4d9d-80c7-02af85c822a8")}};
            Assert.AreEqual(@"{ ""guid"": { ""$uid"": ""936da01f-9abd-4d9d-80c7-02af85c822a8"" } }", 
                            JsonUtils.Serialize(doc));
        }
        
        [Test]
        public void TestSerializeDocWithDBRef(){
            var ostr = "000102030405060708090001";
            var doc = new Document(){{"d", new DBRef("smallreads", new Oid(ostr))}};
            Assert.AreEqual(String.Format(@"{{ ""d"": {{ ""$ref"": ""smallreads"", ""$id"": ""{0}"" }} }}", ostr), 
                            JsonUtils.Serialize(doc));
        }
        
        [Test]
        public void TestSerializeDocWithMinMax(){
            var doc = new Document(){{"mn", MongoMinKey.Value}, {"mx", MongoMaxKey.Value}};
            Assert.AreEqual(@"{ ""mn"": { ""$minkey"": 1 }, ""mx"": { ""$maxkey"": 1 } }", 
                            JsonUtils.Serialize(doc));
        }
        
        [Test]
        public void TestSerializeDocWithCode(){
            var c = new Code("function add(x, y){\n" +
                             "  return x + y;\n" +
                             "}\n");
            var doc = new Document(){{"c", c}};
            Assert.AreEqual(@"{ ""c"": { $code : ""function add(x, y){\n  return x + y;\n}\n"" } }",
                            JsonUtils.Serialize(doc));
        }
            
    }
}
