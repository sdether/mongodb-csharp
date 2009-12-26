using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping
{
    public class DocumentToEntityTranslator
    {
        #region Private Fields

        private MappingStore mappingStore;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentToEntityTranslator"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        public DocumentToEntityTranslator(MappingStore mappingStore)
        {
            if (mappingStore == null)
                throw new ArgumentNullException("mappingStore");

            this.mappingStore = mappingStore;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Translates the document into the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public object Translate(Type type, Document document)
        {
            var classMap = this.mappingStore.GetClassMapFor(type);
            return this.Translate(classMap, document);
        }

        /// <summary>
        /// Translates the specified class map.
        /// </summary>
        /// <param name="classMap">The class map.</param>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public virtual object Translate(ClassMap classMap, Document document)
        {
            //this is a destructive process in order to obtain extended properties.
            //therefore we will work off of a copy in order to preserve the original document.
            var documentCopy = new Document();
            document.CopyTo(documentCopy); 

            if(classMap.IsPolymorphic)
            {
                var discriminator = document[classMap.DiscriminatorKey];
                //TODO: potentially allow for conversion here...
                classMap = classMap.GetClassMapByDiscriminator(discriminator);
            }

            var entity = Activator.CreateInstance(classMap.Type);
            this.Translate(classMap, entity, documentCopy);
            return entity;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Hydrates the specified entity map.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entityMap">The entity map.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="document">The document.</param>
        private void Translate(ClassMap classMap, object entity, Document document)
        {
            if (classMap.HasId)
                this.ApplyIdMap(classMap.IdMap, entity, document);

            this.ApplySimpleMemberMaps(classMap.SimpleMemberMaps, entity, document);
            this.ApplyNestedClassMemberMaps(classMap.NestedClassMemberMaps, entity, document);
            this.ApplyReferenceMemberMaps(classMap.ReferenceMemberMaps, entity, document);

            if (classMap.IsPolymorphic)
                document.Remove(classMap.DiscriminatorKey);

            if(classMap.HasExtendedProperties)
                this.ApplyExtendedPropertiesMap(classMap.ExtendedPropertiesMap, entity, document);
        }

        /// <summary>
        /// Applies the extended properties map.
        /// </summary>
        /// <param name="extendedPropertiesMap">The extended properties map.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="document">The document.</param>
        private void ApplyExtendedPropertiesMap(ExtendedPropertiesMap extendedPropertiesMap, object entity, Document document)
        {
            var dictionary = document.ToDictionary();
            extendedPropertiesMap.MemberSetter(entity, dictionary);
        }

        /// <summary>
        /// Applies the id map.
        /// </summary>
        /// <param name="idMap">The id map.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="document">The document.</param>
        private void ApplyIdMap(IdMap idMap, object entity, Document document)
        {
            var value = document[idMap.Key];
            document.Remove(idMap.Key);
            value = MongoTypeConverter.ConvertFromDocumentValue(value);
            idMap.MemberSetter(entity, value);
        }

        /// <summary>
        /// Applies the nested document member maps.
        /// </summary>
        /// <param name="nestedClassMemberMaps">The nested document member maps.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="document">The document.</param>
        private void ApplyNestedClassMemberMaps(IEnumerable<NestedClassMemberMap> nestedClassMemberMaps, object entity, Document document)
        {
            foreach (var nestedClassMap in nestedClassMemberMaps)
            {
                var value = document[nestedClassMap.Key] as Document;
                document.Remove(nestedClassMap.Key);
                if (value == null)
                    return;

                var nestedEntity = this.Translate(nestedClassMap.NestedClassMap, value);
                nestedClassMap.MemberSetter(entity, nestedEntity);
            }
        }

        /// <summary>
        /// Applies the reference member maps.
        /// </summary>
        /// <param name="referenceMemberMaps">The reference member maps.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="document">The document.</param>
        private void ApplyReferenceMemberMaps(IEnumerable<ReferenceMemberMap> referenceMemberMaps, object entity, Document document)
        {
            foreach (var referenceMemberMap in referenceMemberMaps)
                throw new NotSupportedException("ReferenceMemberMaps are not supported yet.");
        }

        /// <summary>
        /// Applies the simple member maps.
        /// </summary>
        /// <param name="simpleMemberMaps">The simple member maps.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="document">The document.</param>
        private void ApplySimpleMemberMaps(IEnumerable<SimpleMemberMap> simpleMemberMaps, object entity, Document document)
        {
            foreach (var simpleMemberMap in simpleMemberMaps)
            {
                var value = document[simpleMemberMap.Key];
                document.Remove(simpleMemberMap.Key);
                value = MongoTypeConverter.ConvertFromDocumentValue(value);
                if (value == null)
                    value = GetDefaultValue(simpleMemberMap.MemberType);
                simpleMemberMap.MemberSetter(entity, value);
            }
        }

        private static object GetDefaultValue(Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);
            return null;
        }

        #endregion
    }
}