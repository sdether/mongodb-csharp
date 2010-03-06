﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Mapper.Configuration;
using MongoDB.Mapper.Mapping;
using MongoDB.Mapper.Mapping.Visitors;
using MongoDB.Mapper.Tracking;

namespace MongoDB.Mapper.Persistence
{
    public abstract class FindActionBase : PersistenceAction
    {
        #region Private Static Methods

        /// <summary>
        /// Determines whether the query is an identity query.
        /// </summary>
        /// <param name="classMap">The class map.</param>
        /// <param name="conditions">The conditions.</param>
        /// <returns>
        /// 	<c>true</c> if [is find by id] [the specified class map]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsFindById(ClassMapBase classMap, Document conditions)
        {
            return classMap.HasId && conditions.Count == 1 && conditions[classMap.IdMap.Key] != null;
        }

        /// <summary>
        /// Determines whether the query can be called without a cursor.
        /// </summary>
        /// <param name="classMap">The class map.</param>
        /// <param name="conditions">The conditions.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns>
        /// 	<c>true</c> if [is find one] [the specified class map]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsFindOne(int limit, int skip, Document orderBy)
        {
            return orderBy.Count == 0 && limit == 1 && skip == 0;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FindAction"/> class.
        /// </summary>
        /// <param name="mongoSession">The mongoSession.</param>
        /// <param name="mongoSessionCache">The mongo session cache.</param>
        /// <param name="changeTracker">The change tracker.</param>
        public FindActionBase(IMongoSessionImplementor mongoSession, IMongoSessionCache mongoSessionCache, IChangeTracker changeTracker)
            : base(mongoSession, mongoSessionCache, changeTracker)
        {  }

        #endregion

        #region Protected Methods

        protected IEnumerable<object> Find(ClassMapBase classMap, Document conditions, int limit, int skip, Document orderBy, Document fields)
        {
            if (classMap == null)
                throw new ArgumentNullException("classMap");
            conditions = conditions ?? new Document();
            orderBy = orderBy ?? new Document();
            fields = fields ?? new Document();

            var query = this.CreateQuery(conditions, orderBy);
            conditions = (Document)query["query"] ?? query;

            var collection = this.GetCollectionForClassMap(classMap);
            IEnumerable<Document> documents;
            if (IsFindById(classMap, conditions))
            {
                var id = classMap.IdMap.ValueConverter.FromDocument(conditions[classMap.IdMap.Key]);
                object entity = null;
                if(this.MongoSessionCache.TryToFind(classMap.CollectionName, id, out entity))
                    return new[] { entity };

                documents = new[] { collection.FindOne(conditions) };
            }
            else if (IsFindOne(limit, skip, orderBy))
            {
                //if the particular type we need has a discriminator, we need to filter on it...
                if (classMap.IsPolymorphic && classMap.Discriminator != null)
                    conditions[classMap.DiscriminatorKey] = classMap.Discriminator;
                var document = collection.FindOne(conditions);
                if (document == null)
                    documents = Enumerable.Empty<Document>();
                else
                    documents = new[] {  document };
            }
            else
            {
                if (classMap.IsPolymorphic)
                {
                    //if we are projecting, we need to make sure we get the discriminator back as well...
                    if (fields.Count != 0)
                        fields[classMap.DiscriminatorKey] = 1;

                    //if the particular type we need has a discriminator, we need to filter on it...
                    if (classMap.Discriminator != null)
                        conditions[classMap.DiscriminatorKey] = classMap.Discriminator;
                }

                documents = collection.Find(query, limit, skip, fields).Documents;
            }

            //if we are fetching everything and want to track the entities... (0 means everything).
            bool trackEntities = fields.Count == 0;
            return this.CreateEntities(classMap, documents, trackEntities);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates the entities.
        /// </summary>
        /// <param name="classMap">The class map.</param>
        /// <param name="documents">The documents.</param>
        /// <returns></returns>
        private IEnumerable<object> CreateEntities(ClassMapBase classMap, IEnumerable<Document> documents, bool trackEntities)
        {
            foreach (var document in documents)
            {
                ClassMapBase concreteClassMap = classMap;
                if (classMap.IsPolymorphic)
                {
                    object discriminator = document[classMap.DiscriminatorKey];
                    concreteClassMap = classMap.GetClassMapByDiscriminator(discriminator);
                }

                var entity = this.MongoSession.MapToEntity(concreteClassMap, document.Copy());

                if (trackEntities)
                {
                    this.MongoSessionCache.Store(classMap.CollectionName, classMap.GetId(entity), entity);
                    this.ChangeTracker.GetTrackedEntity(entity).MoveToPossibleModified(document);
                }

                yield return entity;
            }
        }

        /// <summary>
        /// Creates the full query.
        /// </summary>
        /// <returns></returns>
        private Document CreateQuery(Document conditions, Document orderBy)
        {
            if (orderBy.Count == 0)
                return conditions;

            return new Document()
                .Append("query", conditions)
                .Append("orderby", orderBy);
        }

        #endregion
    }
}