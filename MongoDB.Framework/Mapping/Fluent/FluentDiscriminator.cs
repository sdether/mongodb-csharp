using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping.Models;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentDiscriminator<TDiscriminator>
    {
        private SuperClassMapModel superClassMapModel;

        public FluentDiscriminator(SuperClassMapModel superClassMapModel)
        {
            this.superClassMapModel = superClassMapModel;
        }

        public FluentDiscriminator<TDiscriminator> SubClass<TSubClass>(TDiscriminator discriminator, Action<FluentSubClassMap<TSubClass>> configure)
        {
            var subClassMap = new FluentSubClassMap<TSubClass>();
            configure(subClassMap);
            subClassMap.Model.Discriminator = discriminator;
            this.superClassMapModel.SubClassMaps.Add(subClassMap.Model);
            return this;
        }
    }
}