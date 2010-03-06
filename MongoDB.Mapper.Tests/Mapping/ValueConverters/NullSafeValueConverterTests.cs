using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;

using Moq;
using NUnit.Framework;

namespace MongoDB.Mapper.Mapping.ValueConverters
{
    public class NullSafeValueConverterTests
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
            public void should_convert_nulls_into_MongoDBNull()
            {
                var converter = new NullSafeValueConverter(typeof(string));
                var result = converter.ToDocument(null);

                Assert.AreEqual(MongoDBNull.Value, result);
            }

            [Test]
            public void should_leave_value_alone_when_not_null()
            {
                var converter = new NullSafeValueConverter(typeof(string));
                var result = converter.ToDocument("Sammy");

                Assert.AreEqual("Sammy", result);
            }
        }

        [TestFixture]
        public class When_converting_from_a_document
        {
            [TestFixture]
            public class Given_the_type_is_a_reference_type
            {
                private IMongoSessionImplementor mongoSession;

                [SetUp]
                public void SetUp()
                {
                    mongoSession = new Mock<IMongoSessionImplementor>().Object;
                }

                [Test]
                public void should_convert_null_into_null()
                {
                    var converter = new NullSafeValueConverter(typeof(Uri));
                    var result = converter.FromDocument(null);

                    Assert.IsNull(result);
                }

                [Test]
                public void should_convert_MongoDBNull_into_null()
                {
                    var converter = new NullSafeValueConverter(typeof(Uri));
                    var result = converter.FromDocument(MongoDBNull.Value);

                    Assert.IsNull(result);
                }

                [Test]
                public void should_leave_value_alone_when_not_null()
                {
                    var converter = new NullSafeValueConverter(typeof(Uri));
                    var result = converter.FromDocument(new Uri("http://localhost"));

                    Assert.AreEqual(new Uri("http://localhost"), result);
                }
            }

            [TestFixture]
            public class Given_the_type_is_a_value_type
            {
                private IMongoSessionImplementor mongoSession;

                [SetUp]
                public void SetUp()
                {
                    mongoSession = new Mock<IMongoSessionImplementor>().Object;
                }

                [Test]
                public void should_convert_null_into_null()
                {
                    var converter = new NullSafeValueConverter(typeof(int));
                    var result = converter.FromDocument(null);

                    Assert.AreEqual(0, result);
                }

                [Test]
                public void should_convert_MongoDBNull_into_null()
                {
                    var converter = new NullSafeValueConverter(typeof(int));
                    var result = converter.FromDocument(MongoDBNull.Value);

                    Assert.AreEqual(0, result);
                }

                [Test]
                public void should_leave_value_alone_when_not_null()
                {
                    var converter = new NullSafeValueConverter(typeof(int));
                    var result = converter.FromDocument(42);

                    Assert.AreEqual(42, result);
                }
            }
        }
    }
}