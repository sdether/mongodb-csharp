using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Tracking;

namespace MongoDB.Framework.Persistence
{
    public abstract class FindActionBase : PersistenceAction
    {
        private ChangeTrackingDocumentToEntityTranslator translator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindAction"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="changeTracker">The change tracker.</param>
        /// <param name="collection">The collection.</param>
        public FindActionBase(MappingStore mappingStore, ChangeTracker changeTracker, IMongoCollection collection)
            : base(mappingStore, changeTracker, collection)
        {
            this.translator = new ChangeTrackingDocumentToEntityTranslator(mappingStore, changeTracker);
        }

        /// <summary>
        /// Creates the entity.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        protected object CreateEntity(Type entityType, Document document)
        {
            return this.translator.Translate(entityType, document);
        }

        private class ChangeTrackingDocumentToEntityTranslator : DocumentToEntityTranslator
        {
            private ChangeTracker changeTracker;

            /// <summary>
            /// Initializes a new instance of the <see cref="ChangeTrackingDocumentToEntityTranslator"/> class.
            /// </summary>
            /// <param name="mappingStore">The mapping store.</param>
            /// <param name="changeTracker">The change tracker.</param>
            public ChangeTrackingDocumentToEntityTranslator(MappingStore mappingStore, ChangeTracker changeTracker)
                : base(mappingStore)
            {
                this.changeTracker = changeTracker;
            }

            /// <summary>
            /// Translates the specified document map.
            /// </summary>
            /// <param name="documentMap">The document map.</param>
            /// <param name="document">The document.</param>
            /// <returns></returns>
            public override object Translate(DocumentMap documentMap, Document document)
            {
                if (documentMap.HasId)
                {
                    var value = document[documentMap.IdMap.Key];
                    value = MongoTypeConverter.ConvertFromDocumentValue(value);
                    TrackedObject trackedObject;
                    if (this.changeTracker.TryGetTrackedObjectById((string)value, out trackedObject))
                        return trackedObject.Current;

                }

                var entity = base.Translate(documentMap, document);

                if (documentMap.HasId)
                    this.changeTracker.Track(document, entity);

                //TODO: may need to fix up references...
                return entity;
            }
        }
    }
}