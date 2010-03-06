using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Mapper.Mapping
{
  public interface IClassActivator
  {
    object Activate(Type type, Document document);
  }
}
