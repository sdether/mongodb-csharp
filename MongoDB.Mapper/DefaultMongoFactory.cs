using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;

namespace MongoDB.Mapper
{
    public class DefaultMongoFactory : IMongoFactory
    {
        /// <summary>
        /// Creates the mongo.
        /// </summary>
        /// <returns></returns>
        public Mongo CreateMongo()
        {
            return MongoFactory.CreateMongo();
        }
    }
}
