using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;

using Moq;
using NUnit.Framework;

namespace MongoDB.Framework.Mapping.Types
{
    public class CollectionValueTypeTests
    {
        [TestFixture]
        public class When_converting_to_a_document
        {
            private IMongoContext mongoContext;

            [SetUp]
            public void SetUp()
            {
                mongoContext = new Mock<IMongoContext>().Object;
            }

            [Test]
            public void should_return_MongoDBNull_when_value_is_null()
            {
                var mockCollectionType = new Mock<ICollectionType>();
                mockCollectionType.Setup(x => x.GetCollectionType(It.IsAny<IValueType>())).Returns(typeof(List<int>));
                var elementValueType = new Mock<IValueType>().Object;
                var valueType = new CollectionValueType(mockCollectionType.Object, elementValueType);
                var result = valueType.ConvertToDocumentValue(null, mongoContext);

                Assert.AreEqual(MongoDBNull.Value, result);
            }

            [Test]
            public void should_return_the_result_from_collection_type_when_value_is_not_null()
            {
                var mockCollectionType = new Mock<ICollectionType>();
                mockCollectionType.Setup(x => x.GetCollectionType(It.IsAny<IValueType>())).Returns(typeof(List<int>));
                mockCollectionType.Setup(x => x.ConvertToDocumentValue(It.IsAny<IValueType>(), It.IsAny<object>(), mongoContext)).Returns(new[] { 1, 2, 3 });
                var elementValueType = new Mock<IValueType>().Object;
                var valueType = new CollectionValueType(mockCollectionType.Object, elementValueType);
                var result = valueType.ConvertToDocumentValue(new[] { 7, 8, 9 }, mongoContext);

                Assert.AreEqual(new[] { 1, 2, 3 }, result);
            }
        }

        [TestFixture]
        public class When_converting_from_a_document
        {
            private IMongoContext mongoContext;

            [SetUp]
            public void SetUp()
            {
                mongoContext = new Mock<IMongoContext>().Object;
            }

            [Test]
            public void should_return_null_when_value_is_null()
            {
                var mockCollectionType = new Mock<ICollectionType>();
                mockCollectionType.Setup(x => x.GetCollectionType(It.IsAny<IValueType>())).Returns(typeof(List<int>));
                var elementValueType = new Mock<IValueType>().Object;
                var valueType = new CollectionValueType(mockCollectionType.Object, elementValueType);
                var result = valueType.ConvertFromDocumentValue(null, mongoContext);

                Assert.IsNull(result);
            }

            [Test]
            public void should_return_the_result_from_collection_type_when_value_is_not_null()
            {
                var mockCollectionType = new Mock<ICollectionType>();
                mockCollectionType.Setup(x => x.GetCollectionType(It.IsAny<IValueType>())).Returns(typeof(List<int>));
                mockCollectionType.Setup(x => x.ConvertFromDocumentValue(It.IsAny<IValueType>(), It.IsAny<object>(), It.IsAny<IMongoContext>())).Returns(new[] { 1, 2, 3 });
                var elementValueType = new Mock<IValueType>().Object;
                var valueType = new CollectionValueType(mockCollectionType.Object, elementValueType);
                var result = valueType.ConvertFromDocumentValue(new[] { 7, 8, 9 }, mongoContext);

                Assert.AreEqual(new[] { 1, 2, 3 }, result);
            }
        }

    }
}