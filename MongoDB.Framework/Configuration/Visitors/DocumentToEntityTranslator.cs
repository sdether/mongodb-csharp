using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Configuration.Visitors
{
    public class DocumentToEntityTranslator : IMapVisitor
    {
        #region Public Static Methods

        /// <summary>
        /// Converts the document to dictionary.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="dictionary">The dictionary.</param>
        public static void ConvertDocumentToDictionary(Document document, IDictionary<string, object> dictionary)
        {
            foreach (string key in document.Keys)
            {
                object value = document[key];
                if (value is Document)
                {
                    var subDictionary = new Dictionary<string, object>();
                    ConvertDocumentToDictionary((Document)value, subDictionary);
                    value = subDictionary;
                }
                dictionary.Add(key, value);
            }
        }

        #endregion

        #region Protected Fields

        protected Document document;
        protected object entity;
        protected IDictionary<string, object> unmappedValues;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <value>The entity.</value>
        public object Entity
        {
            get { return this.entity; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentToEntityTranslator"/> class.
        /// </summary>
        /// <param name="document">The document.</param>
        public DocumentToEntityTranslator(Document document)
        {
            if (document == null)
                throw new ArgumentNullException("document");

            this.document = new Document();
            document.CopyTo(this.document);
            this.unmappedValues = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentToEntityTranslator"/> class.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="entity">The entity.</param>
        public DocumentToEntityTranslator(Document document, object entity)
            : this(document)
        {
            this.entity = entity;
        }

        #endregion

        #region Public Methods

        public void VisitRootEntityMap(RootEntityMap rootEntityMap)
        {
            this.entity = this.CreateInstance(rootEntityMap);
            rootEntityMap.IdMap.Accept(this);
            this.VisitEntityMap(rootEntityMap);
        }

        public void VisitEntityMap(EntityMap entityMap)
        {
            if(this.entity == null)
                this.entity = this.CreateInstance(entityMap);

            if (this.entity.GetType() != entityMap.Type)
            {
                var discriminatedEntityMap = entityMap.GetDiscriminatedEntityMapByType(entity.GetType());
                discriminatedEntityMap.Accept(this);
                this.document.Remove(entityMap.DiscriminatingDocumentKey);
            }
            else if (entityMap.HasDiscriminatingValue)
            {
                this.document.Remove(entityMap.DiscriminatingDocumentKey);
            }

            this.VisitDiscriminatedEntityMap(entityMap);

            if (!entityMap.HasExtendedPropertiesMap)
                return;
            var extProps = new Dictionary<string, object>();
            ConvertDocumentToDictionary(this.document, extProps);
            entityMap.ExtendedPropertiesMap.Setter(entity, extProps);
        }

        public void VisitDiscriminatedEntityMap(DiscriminatedEntityMap discriminatedEntityMap)
        {
            if (this.entity == null)
                throw new NotSupportedException("Discriminated entities cannot be at the root.");

            foreach (var memberMap in discriminatedEntityMap.MemberMaps)
                memberMap.Accept(this);
        }

        public void VisitPrimitiveMemberMap(PrimitiveMemberMap primitiveMemberMap)
        {
            var value = primitiveMemberMap.GetValueFromDocument(this.document);
            primitiveMemberMap.Setter(this.entity, value);
            this.document.Remove(primitiveMemberMap.DocumentKey);
        }

        public void VisitComponentMemberMap(ComponentMemberMap componentMemberMap)
        {
            var subDocument = (Document)componentMemberMap.GetValueFromDocument(this.document);

            var oldEntity = this.entity;
            var oldDocument = this.document;

            this.document = subDocument;
            this.entity = null;
            componentMemberMap.EntityMap.Accept(this);

            componentMemberMap.Setter(oldEntity, entity);
            this.entity = oldEntity;
            this.document = oldDocument;

            this.document.Remove(componentMemberMap.DocumentKey);
        }

        public void VisitIdMap(IdMap idMap)
        {
            var value = idMap.GetValueFromDocument(document);
            idMap.Setter(this.entity, value);
            this.document.Remove(idMap.DocumentKey);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        protected virtual object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates an instance of the 
        /// </summary>
        /// <param name="entityMap">The entity map.</param>
        /// <returns></returns>
        private object CreateInstance(EntityMap entityMap)
        {
            Type entityType = entityMap.Type;
            if (entityMap.IsDiscriminated)
            {
                object discriminatorValue = document[entityMap.DiscriminatingDocumentKey];
                document.Remove(entityMap.DiscriminatingDocumentKey);
                var discriminatedEntityMap = entityMap.GetDiscriminatedEntityMapByValue(discriminatorValue);
                entityType = discriminatedEntityMap.Type;
            }

            return this.CreateInstance(entityType);
        }

        #endregion
    }
}