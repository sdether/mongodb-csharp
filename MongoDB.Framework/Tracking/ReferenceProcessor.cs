using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.Mapping;
using MongoDB.Framework.Mapping.Types;
using MongoDB.Framework.Mapping.Visitors;
using MongoDB.Driver;

namespace MongoDB.Framework.Tracking
{
    public class ReferenceProcessor : DefaultMapVisitor
    {
        private Document currentDocument;
        private object currentEntity;
        private ChangeTracker changeTracker;
        private MappingStore mappingStore;
        private TrackedObject trackedObject;

        public ReferenceProcessor(MappingStore mappingStore, ChangeTracker changeTracker, TrackedObject trackedObject)
        {
            if (changeTracker == null)
                throw new ArgumentNullException("changeTracker");
            if (mappingStore == null)
                throw new ArgumentNullException("mappingStore");
            if (trackedObject == null)
                throw new ArgumentNullException("trackedObject");

            this.changeTracker = changeTracker;
            this.mappingStore = mappingStore;
            this.trackedObject = trackedObject;
            this.currentDocument = trackedObject.Original;
            this.currentEntity = trackedObject.Current;
        }

        public void Process()
        {
            var classMap = this.mappingStore.GetClassMapFor(this.trackedObject.Current.GetType());
            classMap.Accept(this);
        }
    }
}