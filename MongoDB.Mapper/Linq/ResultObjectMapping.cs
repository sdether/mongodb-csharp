using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Remotion.Data.Linq.Clauses;

namespace MongoDB.Mapper.Linq
{
    public class ResultObjectMapping : IEnumerable<KeyValuePair<IQuerySource, object>>
    {
        private readonly Dictionary<IQuerySource, object> resultObjectsBySource = 
            new Dictionary<IQuerySource, object>();

        public void Add(IQuerySource querySource, object resultObject)
        {
            this.resultObjectsBySource.Add(querySource, resultObject);
        }

        public T GetObject<T>(IQuerySource source)
        {
            return (T)this.resultObjectsBySource[source];
        }

        public IEnumerator<KeyValuePair<IQuerySource, object>> GetEnumerator()
        {
            return resultObjectsBySource.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
