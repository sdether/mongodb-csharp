using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Moq;

namespace MongoDB.Framework.Configuration.Mapping.IdGenerators
{
    [TestFixture]
    public class AssignedGeneratorTests
    {
        [Test]
        public void Should_return_entity_id_when_set()
        {
            var generator = new AssignedGenerator();
            var idMap = new IdMap("Id", x => 42, (x, y) => { }, new Mock<IValueType>().Object, generator, null);
            var mockClassMap = new Mock<ClassMap>(typeof(int), Enumerable.Empty<MemberMap>(), Enumerable.Empty<ManyToOneMap>(), null);
            var mockMongoContext = new Mock<IMongoContextImplementor>();
            mockMongoContext.Setup(x => x.MappingStore.GetClassMapFor(It.IsAny<Type>())).Returns(mockClassMap.Object);
            mockClassMap.SetupGet(x => x.IdMap).Returns(idMap);
            var id = (int)generator.Generate("setat", mockMongoContext.Object);

            Assert.AreEqual(42, id);
        }

        [Test]
        [ExpectedException(ExceptionType=typeof(IdGenerationException))]
        public void Should_throw_id_generation_exception_when_id_has_not_been_assigned()
        {
            var generator = new AssignedGenerator();
            var idMap = new IdMap("Id", x => null, (x, y) => { }, new Mock<IValueType>().Object, generator, null);
            var mockClassMap = new Mock<ClassMap>(typeof(int), Enumerable.Empty<MemberMap>(), Enumerable.Empty<ManyToOneMap>(), null);
            var mockMongoContext = new Mock<IMongoContextImplementor>();
            mockMongoContext.Setup(x => x.MappingStore.GetClassMapFor(It.IsAny<Type>())).Returns(mockClassMap.Object);
            mockClassMap.SetupGet(x => x.IdMap).Returns(idMap);
            generator.Generate("setat", mockMongoContext.Object);
        }
    }
}