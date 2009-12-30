using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Moq;
using NUnit.Framework;

namespace MongoDB.Framework.Mapping
{
    [TestFixture]
    public class IdMapTests
    {
        [Test]
        public void Should_map_from_document()
        {
            var vt = new Mock<IValueType>().Object;
            var gen = new Mock<IIdGenerator>().Object;
            Guid id = Guid.NewGuid();
            IdMap map = new IdMap("Id", e => id, (e, eid) => id = (Guid)eid, vt, gen, Guid.Empty);

        }
    }
}