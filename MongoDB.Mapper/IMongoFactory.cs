using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Mapper
{
    public interface IMongoFactory
    {
        /// <summary>
        /// Creates the mongo.
        /// </summary>
        /// <returns></returns>
        Mongo CreateMongo();
    }
}