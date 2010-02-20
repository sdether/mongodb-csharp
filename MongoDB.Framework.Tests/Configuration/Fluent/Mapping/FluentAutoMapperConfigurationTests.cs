using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.Configuration.Mapping;

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
                .AutoMapAsRootClass<MappingTests.PartyMap>(auto =>
                {
                    auto.Id.IsNamed("Id");
                    auto.Discriminator.KeyIs("type");
                    auto.Map.AllPublicProperties().ExceptFor(m => m.Name == "Id");
                })
                .WithAssemblyContainingType<MappingTests.PartyMap>(assembly =>
                {
                    assembly.AutoMapAsSubClassesTypesDerivedFrom<MappingTests.PartyMap>(auto =>
                    {
                        auto.Id.IsNamed("Id");
                        auto.Discriminator.KeyIs("type").ValueIs(t => t.FullName);
                        auto.Map.AllDeclaredPublicProperties().ExceptFor(m => m.Name == "Id");
                    });
                }).BuildMappingStore();
        }
    }
}