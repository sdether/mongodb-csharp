using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Models
{
    public class RootClassMapModel : SuperClassMapModel
    {
        public string CollectionName { get; set; }

        public IdMapModel IdMap { get; set; }

        public RootClassMapModel(Type type)
            : base(type)
        { }
    }
}
