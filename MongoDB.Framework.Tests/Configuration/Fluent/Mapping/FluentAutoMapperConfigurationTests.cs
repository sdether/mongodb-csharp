using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Configuration.Mapping.Auto;

using NUnit.Framework;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    [TestFixture]
    public class FluentAutoMapperConfigurationTests
    {

        [Test]
        public void Syntax()
        {
            var mappingStore = new FluentMapModelRegistry()
                .AddSource(AutoMap.FromAssemblyOf<MappingTests.Party>()
                    .Where(t => typeof(MappingTests.Party).IsAssignableFrom(t) || t == typeof(MappingTests.PhoneNumber))
                    .IncludeType<MappingTests.Party>()
                    .SetupExpressions(x =>
                    {
                        x.IsRootClass = t => t == typeof(MappingTests.Party);
                        x.IsNestedClass = t => t == typeof(MappingTests.PhoneNumber);
                    }))
                .BuildMappingStore();
        }
    }
}