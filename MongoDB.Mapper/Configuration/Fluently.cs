using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Mapper.Configuration.Fluent;

namespace MongoDB.Mapper.Configuration
{
    public static class Fluently
    {
        public static FluentConfiguration Configure()
        {
            return new FluentConfiguration();
        }
    }
}