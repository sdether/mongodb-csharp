using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public class DefaultClassActivatorConvention : IClassActivatorConvention
    {
        public static readonly DefaultClassActivatorConvention AlwaysMatching = new DefaultClassActivatorConvention();

        private  DefaultClassActivatorConvention()
        { }

        public bool CanCreateActivator(Type type)
        {
            //TODO: this should check to see if there is a default constructor...
            return true;
        }

        public IClassActivator CreateActivator(Type type)
        {
            return new DefaultClassActivator();
        }

        public bool Matches(Type topic)
        {
            return true;
        }
    }
}
