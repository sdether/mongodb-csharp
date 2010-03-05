using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Configuration.Fluent;

namespace MongoDB.Framework.Configuration
{
    public static class Fluently
    {
        public static FluentConfiguration Configure()
        {
            return new FluentConfiguration();
        }
    }
}