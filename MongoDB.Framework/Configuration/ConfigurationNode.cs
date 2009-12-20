using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration
{
    public abstract class ConfigurationNode : IMapVisitable
    {
        public abstract void Accept(IMapVisitor visitor);
    }
}