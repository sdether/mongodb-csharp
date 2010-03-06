using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MongoDB.Mapper.Proxy
{
    public static class IdentityEqualityComparer
    {
        public static int GetHashCode(object obj)
        {
            throw new NotImplementedException();
        }

        public static bool Equals(object id1, object id2)
        {
            return false;
        }
    }
}