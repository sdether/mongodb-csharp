using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Tracking
{
    public enum TrackedObjectState
    {
        Added,
        PossiblyModified,
        Modified,
        Removed,
        Dead
    }
}
