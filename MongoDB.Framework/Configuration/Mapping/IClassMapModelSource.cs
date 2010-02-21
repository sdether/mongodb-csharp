using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping
{
    public interface IClassMapModelSource
    {
        IEnumerable<ClassMapModel> GetClassMapModels();
    }
}