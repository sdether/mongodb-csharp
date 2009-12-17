using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;

namespace MongoDB.Framework
{
    public class EntityMapper
    {
        #region Private Static Methods

        public static void ConvertDictionaryToDocument(IDictionary<string, object> dictionary, Document document)
        {
            foreach (var kvp in dictionary)
            {
                if (kvp.Value is IDictionary<string, object>)
                {
                    var subDocument = new Document();
                    ConvertDictionaryToDocument((IDictionary<string, object>)kvp.Value, subDocument);
                    document.Add(kvp.Key, subDocument);
                }
                else
                    document.Add(kvp.Key, kvp.Value);
            }
        }

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

        #region Public Properties

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public MongoConfiguration Configuration { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMapper"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public EntityMapper(MongoConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            this.Configuration = configuration;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Maps the document to entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public object MapDocumentToEntity(Document document, Type entityType)
        {
            var rootEntityMap = this.Configuration.GetRootEntityMapFor(entityType);
            return new DocumentToEntityRunner(document, rootEntityMap).Map();
        }

        /// <summary>
        /// Maps the entity to document.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public Document MapEntityToDocument(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var rootEntityMap = this.Configuration.GetRootEntityMapFor(entity.GetType());

            Document document = new Document();

            Oid id = (Oid)rootEntityMap.IdMap.GetDocumentValueFromEntity(entity);
            if (id != null)
                document.Add("_id", id);
            this.MapEntityToDocument(rootEntityMap, entity, document);

            return document;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Maps the root entity to document.
        /// </summary>
        /// <param name="rootEntityMap">The root entity map.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="document">The document.</param>
        private void MapEntityToDocument(EntityMap entityMap, object entity, Document document)
        {
            Type entityType = entity.GetType();
            this.MapPartialEntityToDocument(entityMap, entity, document);
            if (entityMap.Type != entityType)
            {
                //we must be a discriminated entity
                var discriminatedEntityMap = entityMap.GetDiscriminatedEntityMapByType(entityType);
                this.MapPartialEntityToDocument(discriminatedEntityMap, entity, document);

                document.Add(entityMap.DiscriminateDocumentKey, discriminatedEntityMap.DiscriminatingValue);
            }
            else if (entityMap.IsDiscriminated)
            {
                //we are the root of a non-abstract tree
                document.Add(entityMap.DiscriminateDocumentKey, entityMap.DiscriminatingValue);
            }

            if (entityMap.HasExtendedPropertiesMap)
            {
                var extendedProperties = (IDictionary<string, object>)entityMap.ExtendedPropertiesMap.Getter(entity);
                if (extendedProperties != null)
                    ConvertDictionaryToDocument(extendedProperties, document);
            }
        }

        /// <summary>
        /// Maps the entity to document.
        /// </summary>
        /// <param name="discriminatedEntityMap">The discriminated entity map.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="document">The document.</param>
        private void MapPartialEntityToDocument(DiscriminatedEntityMap discriminatedEntityMap, object entity, Document document)
        {
            foreach (var memberMap in discriminatedEntityMap.MemberMaps)
            {
                var value = memberMap.GetDocumentValueFromEntity(entity);
                if (memberMap is EntityMemberMap)
                {
                    var subDocument = new Document();
                    this.MapPartialEntityToDocument(((EntityMemberMap)memberMap).EntityMap, value, subDocument);
                    document.Add(memberMap.DocumentKey, subDocument);
                }
                else
                {
                    document.Add(memberMap.DocumentKey, memberMap.Converter.ConvertToDocumentValue(value));
                }
            }
        }

        #endregion

        #region Private Class : DocumentToEntityRunner

        private class DocumentToEntityRunner
        {
            #region Private Fields

            private Document document;
            private EntityMap entityMap;
            private DiscriminatedEntityMap discriminatedEntityMap;
            private IDictionary<string, object> unmappedValues;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="DocumentToEntityRunner"/> class.
            /// </summary>
            /// <param name="document">The document.</param>
            /// <param name="entityMap">The entity map.</param>
            public DocumentToEntityRunner(Document document, EntityMap entityMap)
            {
                this.document = document;
                this.entityMap = entityMap;
                this.unmappedValues = new Dictionary<string, object>();
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Maps the document to the entity.
            /// </summary>
            /// <returns></returns>
            public object Map()
            {
                object entity = this.CreateEntity(this.document);

                if (this.entityMap is RootEntityMap)
                {
                    var rootEntityMap = (RootEntityMap)this.entityMap;
                    rootEntityMap.IdMap.SetDocumentValueOnEntity(entity, this.document["_id"]);
                    this.document.Remove("_id");
                }
                this.ApplyMemberMaps(entity);
                this.ApplyExtendedProperties(entity);

                return entity;
            }

            #endregion

            #region Private Methods

            /// <summary>
            /// Applies the extended properties.
            /// </summary>
            /// <param name="entity">The entity.</param>
            private void ApplyExtendedProperties(object entity)
            {
                if (!this.entityMap.HasExtendedPropertiesMap)
                    return;

                var extProps = new Dictionary<string, object>();

                foreach (var unmappedValue in this.unmappedValues)
                    extProps.Add(unmappedValue.Key, unmappedValue.Value);

                this.entityMap.ExtendedPropertiesMap.Setter(entity, extProps);
            }

            /// <summary>
            /// Applies the member maps.
            /// </summary>
            /// <param name="entity">The entity.</param>
            private void ApplyMemberMaps(object entity)
            {
                var combinedMemberMaps = this.entityMap.MemberMaps.ToDictionary(m => m.DocumentKey);
                var disciminatedMemberMaps = new Dictionary<string, MemberMap>();
                if (this.discriminatedEntityMap != null)
                {
                    foreach(var memberMap in this.discriminatedEntityMap.MemberMaps)
                        combinedMemberMaps[memberMap.DocumentKey] = memberMap;
                }

                foreach (string key in this.document.Keys)
                {
                    if (combinedMemberMaps.ContainsKey(key))
                        this.ApplyMemberMap(document, entity, combinedMemberMaps[key]);
                    else
                    {
                        object value = document[key];
                        if (value is Document)
                        {
                            var dictionary = new Dictionary<string, object>();
                            ConvertDocumentToDictionary((Document)value, dictionary);
                            value = dictionary;
                        }
                        this.unmappedValues.Add(key, value);
                    }
                }
            }

            /// <summary>
            /// Applies the member map.
            /// </summary>
            /// <param name="entity">The entity.</param>
            /// <param name="map">The map.</param>
            private void ApplyMemberMap(Document document, object entity, MemberMap memberMap)
            {
                object value = document[memberMap.DocumentKey];

                EntityMemberMap entityMemberMap = memberMap as EntityMemberMap;
                if (entityMemberMap != null)
                {
                    value = new DocumentToEntityRunner((Document)value, entityMemberMap.EntityMap).Map();
                }

                memberMap.SetDocumentValueOnEntity(entity, value);
            }

            /// <summary>
            /// Creates the entity.
            /// </summary>
            /// <param name="document">The document.</param>
            /// <returns></returns>
            private object CreateEntity(Document document)
            {
                Type entityType = this.entityMap.Type;
                if (this.entityMap.IsDiscriminated)
                {
                    object discriminatorValue = document[this.entityMap.DiscriminateDocumentKey];
                    document.Remove(this.entityMap.DiscriminateDocumentKey);
                    this.discriminatedEntityMap = this.entityMap.GetDiscriminatedEntityMapByValue(discriminatorValue);
                    entityType = this.discriminatedEntityMap.Type;
                }

                return Activator.CreateInstance(entityType);
            }

            #endregion
        }

        #endregion
    }
}