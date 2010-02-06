using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;

using Moq;
using NUnit.Framework;

namespace MongoDB.Framework.Mapping.ValueConverters
{
    public class GuidValueConverterTests
    {
        [TestFixture]
        public class When_converting_to_a_document
        {
            private IMongoSessionImplementor mongoSession;

            [SetUp]
            public void SetUp()
            {
                mongoSession = new Mock<IMongoSessionImplementor>().Object;
            }

            [Test]
            public void should_return_MongoDBNull_when_value_is_null()
            {
                var converter = new GuidValueConverter();
                var result = converter.ToDocument(null);

                Assert.AreEqual(MongoDBNull.Value, result);
            }

            [Test]
            public void should_return_string_when_value_is_not_null()
            {
                var converter = new GuidValueConverter();
                var result = (Binary)converter.ToDocument(Guid.Empty);

                Assert.AreEqual(Guid.Empty.ToByteArray(), result.Bytes);
                Assert.AreEqual(Binary.TypeCode.Uuid, result.Subtype);
            }
        }

        [TestFixture]
        public class When_converting_from_a_document
        {
            private IMongoSessionImplementor mongoSession;

            [SetUp]
            public void SetUp()
            {
                mongoSession = new Mock<IMongoSessionImplementor>().Object;
            }

            [Test]
            public void should_return_an_empty_guid_when_value_is_null()
            {
                var converter = new GuidValueConverter();
                var result = converter.FromDocument(null);

                Assert.AreEqual(Guid.Empty, result);
            }

            [Test]
            public void should_return_a_guid_when_value_is_valid()
            {
                var guid = Guid.NewGuid();
                var converter = new GuidValueConverter();
                var result = converter.FromDocument(new Binary(guid.ToByteArray()) { Subtype = Binary.TypeCode.Uuid });

                Assert.AreEqual(guid, result);
            }
        }
    }
}