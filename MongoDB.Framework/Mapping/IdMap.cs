using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping
{
    public class IdMap : SimpleMemberMap
    {
        public IdMap()
        {
            this.Key = "_id";
            this.PersistNulls = false;
        }
    }
}