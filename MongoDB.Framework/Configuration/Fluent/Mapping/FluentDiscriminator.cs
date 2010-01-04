using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Configuration.Mapping.Models;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    public class FluentDiscriminator<TDiscriminator>
    {
        private SuperClassMapModel superClassMapModel;

        public FluentDiscriminator(SuperClassMapModel superClassMapModel)
        {
            this.superClassMapModel = superClassMapModel;
        }

        public FluentDiscriminator<TDiscriminator> SubClass<TSubClass>(TDiscriminator discriminator, Action<FluentSubClass<TSubClass>> configure)
        {
            var subClassMap = new FluentSubClass<TSubClass>();
            configure(subClassMap);
            subClassMap.Model.Discriminator = discriminator;
            this.superClassMapModel.SubClassMaps.Add(subClassMap.Model);
            return this;
        }
    }
}