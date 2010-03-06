using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper.Tracking
{
    public enum TrackedEntityState
    {
        PossiblyModified,
        Inserted,
        Modified,
        Deleted
    }
}