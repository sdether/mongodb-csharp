using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    [TestFixture]
    public class FluentAutoMapperConfigurationTests
    {

        public void Syntax()
        {

            new FluentMapModelRegistry()
                .WithAssemblyContainingType<string>()
                .AutoMapTypesOf<Uri>(c =>
                {
                    c.Discriminator.KeyIs("type").AndValueIs(t => t.FullName);
                });

        }

    }
}
