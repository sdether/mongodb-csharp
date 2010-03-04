using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace MongoDB.Framework.Mapping.Auto
{
    public class AutoMapperTests
    {
        public void Syntax_exploration()
        {
            //new AutoMapper()
            //    .For<Entity>(x => x.Address, opt => opt.AsReference()

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