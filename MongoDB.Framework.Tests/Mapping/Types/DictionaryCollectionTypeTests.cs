using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;

using Moq;
using NUnit.Framework;

namespace MongoDB.Framework.Mapping.Types
{
    public class DictionaryCollectionTypeTests
    {
        [TestFixture]
        public class When_getting_collection_type
        {
            [Test]
            public void should_return_generic_dictionary_type_using_elements_type()
            {
                var collectionType = new DictionaryCollectionType();
                var elementValueType = new Mock<IValueType>();
                elementValueType.SetupGet(evt => evt.Type).Returns(typeof(int));

                Assert.AreEqual(typeof(Dictionary<string, int>), collectionType.GetCollectionType(elementValueType.Object));
            }
        }

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
            public void should_return_null_when_value_is_null()
            {
                var collectionType = new DictionaryCollectionType();
                var elementValueType = new Mock<IValueType>();

                var result = collectionType.ConvertToDocumentValue(elementValueType.Object, null, mongoContext);

                Assert.IsNull(result);
            }

            [Test]
            public void should_return_an_empty_document_when_value_is_empty()
            {
                var collectionType = new DictionaryCollectionType();
                var elementValueType = new Mock<IValueType>();
                elementValueType.SetupGet(evt => evt.Type).Returns(typeof(int));

                var result = collectionType.ConvertToDocumentValue(elementValueType.Object, new Dictionary<string, int>(), mongoContext);

                Assert.AreEqual(new Document(), result);
            }

            [Test]
            public void should_return_a_document_when_value_is_not_null()
            {
                var collectionType = new DictionaryCollectionType();
                var elementValueType = new Mock<IValueType>();
                elementValueType.SetupGet(evt => evt.Type).Returns(typeof(int));
                elementValueType.Setup(evt => evt.ConvertToDocumentValue(It.IsAny<int>(), mongoContext)).Returns<int, IMongoContext>((i, mc) => i);

                var result = (Document)collectionType.ConvertToDocumentValue(elementValueType.Object, new Dictionary<string, int> { { "one", 1 }, { "two", 2 }, { "three", 3 } }, mongoContext);

                Assert.AreEqual(1, result["one"]);
                Assert.AreEqual(2, result["two"]);
                Assert.AreEqual(3, result["three"]);
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
                var collectionType = new DictionaryCollectionType();
                var elementValueType = new Mock<IValueType>();

                var result = collectionType.ConvertFromDocumentValue(elementValueType.Object, null, mongoContext);

                Assert.IsNull(result);
            }

            [Test]
            public void should_return_a_list_when_value_exists()
            {
                var collectionType = new DictionaryCollectionType();
                var elementValueType = new Mock<IValueType>();
                elementValueType.SetupGet(evt => evt.Type).Returns(typeof(int));
                elementValueType.Setup(evt => evt.ConvertFromDocumentValue(It.IsAny<int>(), mongoContext)).Returns<int, IMongoContext>((i, mc) => i);

                var result = (Dictionary<string, int>)collectionType.ConvertFromDocumentValue(elementValueType.Object, new Document().Append("one", 1).Append("two", 2).Append("three", 3), mongoContext);

                Assert.AreEqual(1, result["one"]);
                Assert.AreEqual(2, result["two"]);
                Assert.AreEqual(3, result["three"]);
            }
        }
    }
}
