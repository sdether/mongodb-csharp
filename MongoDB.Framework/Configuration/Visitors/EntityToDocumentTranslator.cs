using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Configuration.Visitors
{
    public class EntityToDocumentTranslator : IMapVisitor
    {
        #region Public Static Methods

        /// <summary>
        /// Converts the dictionary to document.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="document">The document.</param>
        public static void TranslateDictionaryToDocument(IDictionary<string, object> dictionary, Document document)
        {
            foreach (var kvp in dictionary)
            {
                if (kvp.Value is IDictionary<string, object>)
                {
                    var subDocument = new Document();
                    TranslateDictionaryToDocument((IDictionary<string, object>)kvp.Value, subDocument);
                    document.Add(kvp.Key, subDocument);
                }
                else
                    document.Add(kvp.Key, kvp.Value);
            }
        }

        #endregion

        #region Protected Fields

        protected Document document;
        protected object entity;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the document.
        /// </summary>
        /// <value>The document.</value>
        public Document Document
        {
            get { return this.document; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityToDocumentMapVisitor"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public EntityToDocumentTranslator(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            this.document = new Document();
            this.entity = entity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityToDocumentMapVisitor"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="document">The document.</param>
        public EntityToDocumentTranslator(object entity, Document document)
            : this(entity)
        {
            this.document = document;
        }

        #endregion

        #region Public Methods

        public void VisitRootEntityMap(RootEntityMap rootEntityMap)
        {
            rootEntityMap.IdMap.Accept(this);
            this.VisitEntityMap(rootEntityMap);
        }

        public void VisitEntityMap(EntityMap entityMap)
        {
            if (this.entity.GetType() != entityMap.Type)
            {
                var discriminatedEntityMap = entityMap.GetDiscriminatedEntityMapByType(entity.GetType());
                discriminatedEntityMap.Accept(this);
                this.document[entityMap.DiscriminatingDocumentKey] = discriminatedEntityMap.DiscriminatingValue;
            }
            else if(entityMap.HasDiscriminatingValue)
            {
                this.document[entityMap.DiscriminatingDocumentKey] = entityMap.DiscriminatingValue;
            }

            if (entityMap.HasExtendedPropertiesMap)
            {
                IDictionary<string, object> props = (IDictionary<string, object>)entityMap.ExtendedPropertiesMap.Getter(this.entity);
                TranslateDictionaryToDocument(props, this.document);
            }

            this.VisitDiscriminatedEntityMap(entityMap);
        }

        public void VisitDiscriminatedEntityMap(DiscriminatedEntityMap discriminatedEntityMap)
        {
            foreach (var memberMap in discriminatedEntityMap.MemberMaps)
                memberMap.Accept(this);
        }

        public void VisitPrimitiveMemberMap(PrimitiveMemberMap primitiveMemberMap)
        {
            var value = primitiveMemberMap.Getter(this.entity);
            primitiveMemberMap.SetValueOnDocument(value, this.document);
            this.document[primitiveMemberMap.DocumentKey] = value;
        }

        public void VisitComponentMemberMap(ComponentMemberMap componentMemberMap)
        {
            var oldDocument = this.document;
            var oldEntity = this.entity;
            this.document = new Document();
            this.entity = componentMemberMap.Getter(oldEntity);
            if (this.entity != null)
            {
                componentMemberMap.EntityMap.Accept(this);
                componentMemberMap.SetValueOnDocument(this.document, oldDocument);
            }
            this.document = oldDocument;
            this.entity = oldEntity;
        }

        public void VisitIdMap(IdMap idMap)
        {
            var value = (string)idMap.Getter(this.entity);
            idMap.SetValueOnDocument(value, this.document);
        }

        #endregion
    }
}