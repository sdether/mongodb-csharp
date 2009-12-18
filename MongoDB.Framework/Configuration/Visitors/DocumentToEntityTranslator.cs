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

        #endregion

        #region Public Methods

        public void VisitRootEntityMap(RootEntityMap rootEntityMap)
        {
            this.entity = this.CreateInstance(rootEntityMap);
            rootEntityMap.IdMap.Accept(this);
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

            foreach (var componentMap in discriminatedEntityMap.ComponentMaps)
                componentMap.Accept(this);
            foreach (var memberMap in discriminatedEntityMap.MemberMaps)
                memberMap.Accept(this);
        }

        public void VisitMemberMap(MemberMap memberMap)
        {
            memberMap.SetDocumentValueOnEntity(this.entity, this.document[memberMap.DocumentKey]);
            this.document.Remove(memberMap.DocumentKey);
        }

        public void VisitComponentMap(ComponentMap componentMap)
        {
            var subDocument = document[componentMap.DocumentKey] as Document;
            if (subDocument == null)
                return;

            var oldEntity = this.entity;
            var oldDocument = this.document;

            this.document = subDocument;
            this.entity = null;
            componentMap.EntityMap.Accept(this);

            componentMap.Setter(oldEntity, entity);
            this.entity = oldEntity;
            this.document = oldDocument;

            this.document.Remove(componentMap.DocumentKey);
        }

        public void VisitIdMap(IdMap idMap)
        {
            idMap.SetDocumentValueOnEntity(this.entity, this.document["_id"]);
            this.document.Remove("_id");
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