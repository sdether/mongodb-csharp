using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using MongoDB.Framework.Configuration;

namespace MongoDB.Framework.Mapping.Auto
{
    public class AutoMapperTests
    {
        public void Syntax_exploration()
        {
            var mappingStore = new AutoMappingStore(new AutoMapper());
        }


        private class Entity
        {
            public Address Address { get; set; }
        }

        private class Address
        {

        }
    }
}