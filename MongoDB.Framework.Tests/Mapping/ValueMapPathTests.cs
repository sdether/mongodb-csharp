using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using MongoDB.Framework.Mapping.Fluent;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping
{
    [TestFixture]
    public class ValueMapPathTests
    {
        [Test]
        public void Should_create_document_key()
        {
            var mappingStore = new FluentMappingStore()
                .AddMapsFromAssemblyContaining<PartyMap>();

            var valueMapPath = new ValueMapPath<Person>(mappingStore, "PhoneNumber", "AreaCode");

            Assert.AreEqual("PhoneNumber.AreaCode", valueMapPath.Key);
        }

        [Test]
        public void Should_create_value_for_SimpleValueMap()
        {
            var mappingStore = new FluentMappingStore()
                .AddMapsFromAssemblyContaining<PartyMap>();

            var valueMapPath = new ValueMapPath<Person>(mappingStore, "PhoneNumber", "AreaCode");
            var value = valueMapPath.ConvertToDocumentValue("143");
            Assert.AreEqual("143", value);
        }

        [Test]
        public void Should_create_value_for_NestedDocumentValueMap()
        {
            var mappingStore = new FluentMappingStore()
                .AddMapsFromAssemblyContaining<PartyMap>();

            var valueMapPath = new ValueMapPath<Person>(mappingStore, "PhoneNumber");
            Document value = (Document)valueMapPath.ConvertToDocumentValue(new PhoneNumber() { AreaCode = "111", Prefix = "222", Number = "3333" });

            Assert.AreEqual("111", value["AreaCode"]);
            Assert.AreEqual("222", value["Prefix"]);
            Assert.AreEqual("3333", value["Number"]);
        }
    }
}