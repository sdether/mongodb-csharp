using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;

namespace MongoDB.Framework
{
    public class DefaultMongoFactory : IMongoFactory
    {
        /// <summary>
        /// Creates the mongo.
        /// </summary>
        /// <returns></returns>
        public Mongo CreateMongo()
        {
            return new Mongo();
        }
    }
}
