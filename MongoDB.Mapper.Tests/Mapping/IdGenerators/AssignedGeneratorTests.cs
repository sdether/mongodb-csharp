using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Moq;

namespace MongoDB.Mapper.Mapping.IdGenerators
{
    [TestFixture]
    public class AssignedGeneratorTests
    {
        [Test]
        public void Should_return_entity_id_when_set()
        {
            var generator = new AssignedGenerator();
            var idMap = new IdMap("Id", x => 42, (x, y) => { }, generator, new Mock<IValueConverter>().Object, null);
            var mockClassMap = new Mock<ClassMapBase>(typeof(int));
            var mockMongoSession = new Mock<IMongoSessionImplementor>();
            mockMongoSession.Setup(x => x.MappingStore.GetClassMapFor(It.IsAny<Type>())).Returns(mockClassMap.Object);
            mockClassMap.SetupGet(x => x.IdMap).Returns(idMap);
            var id = (int)generator.Generate("setat", mockMongoSession.Object);

            Assert.AreEqual(42, id);
        }

        [Test]
        [ExpectedException(ExceptionType=typeof(IdGenerationException))]
        public void Should_throw_id_generation_exception_when_id_has_not_been_assigned()
        {
            var generator = new AssignedGenerator();
            var idMap = new IdMap("Id", x => null, (x, y) => { }, generator, new Mock<IValueConverter>().Object, null);
            var mockClassMap = new Mock<ClassMapBase>(typeof(int));
            var mockMongoSession = new Mock<IMongoSessionImplementor>();
            mockMongoSession.Setup(x => x.MappingStore.GetClassMapFor(It.IsAny<Type>())).Returns(mockClassMap.Object);
            mockClassMap.SetupGet(x => x.IdMap).Returns(idMap);
            generator.Generate("setat", mockMongoSession.Object);
        }
    }
}