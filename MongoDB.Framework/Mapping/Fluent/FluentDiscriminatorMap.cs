using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping.Model;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentDiscriminatorMap<TDiscriminator>
    {
        private SuperClassMapModel superClassMapModel;

        public FluentDiscriminatorMap(SuperClassMapModel superClassMapModel)
        {
            this.superClassMapModel = superClassMapModel;
        }

        public FluentDiscriminatorMap<TDiscriminator> SubClass<TSubClass>(TDiscriminator discriminator, Action<FluentSubClassMap<TSubClass>> configure)
        {
            var subClassMap = new FluentSubClassMap<TSubClass>();
            configure(subClassMap);
            subClassMap.Model.Discriminator = discriminator;
            this.superClassMapModel.SubClassMaps.Add(subClassMap.Model);
            return this;
        }
    }
}