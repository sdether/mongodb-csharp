using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Configuration;

namespace MongoDB.Framework
{
    public interface IMongoContextImplementor : IMongoContext
    {
        IMongoConfiguration Configuration { get; }
    }
}