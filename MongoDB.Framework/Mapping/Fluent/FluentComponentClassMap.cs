using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Linq.Visitors;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentComponentClassMap<T> : FluentSuperClassMap<ComponentClassMap, T>
    {
        private ComponentClassMap instance;

        public override ComponentClassMap Instance
        {
            get { return instance; }
        }

        public FluentComponentClassMap()
        {
            this.instance = new ComponentClassMap(typeof(T));
        }
    }

}