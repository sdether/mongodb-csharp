using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace MongoDB.Framework.Configuration.Fluent
{
    [TestFixture]
    public class FluentRootEntityMapTests
    {
        private class TestEntity
        {
            public string Id { get; set; }
        }

        public void Should_map_id()
        {
            FluentRootEntityMap<TestEntity> map = new FluentRootEntityMap<TestEntity>();
            map.Id(m => m.Id);

            Assert.IsNotNull(map.Instance.IdMap);
        }
    }
}