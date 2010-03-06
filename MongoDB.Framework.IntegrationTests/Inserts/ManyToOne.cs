﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Configuration.Fluent.Mapping;
using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Mapping;

using NUnit.Framework;

namespace MongoDB.Framework.Inserts
{
    [TestFixture]
    public class ManyToOne : TestCase
    {
        protected override IMapModelRegistry MapModelRegistry
        {
            get 
            {
                return new FluentMapModelRegistry()
                    .AddMap(new EntityMap())
                    .AddMap(new EntityRefMap());
            }
        }

        protected override void AfterTest()
        {
            using (var mongoSession = this.OpenMongoSession())
            {
                mongoSession.Database.MetaData.DropCollection("Entity");
                mongoSession.Database.MetaData.DropCollection("EntityRef");
            }
        }

        [Test]
        public void Should_insert()
        {
            var reference = new EntityRef();
            var entity = new Entity();
            entity.Reference = reference;
            using (var mongoSession = this.OpenMongoSession())
            {
                mongoSession.InsertOnSubmit(reference);
                mongoSession.InsertOnSubmit(entity);
                mongoSession.SubmitChanges();
            }

            Document insertedDocument;
            using (var mongoSession = this.OpenMongoSession())
            {
                insertedDocument = mongoSession.Database.GetCollection("Entity").FindOne(null);
            }

            Assert.IsNotNull(insertedDocument);
            Assert.AreEqual(entity.Id, insertedDocument["_id"]);
            Assert.AreEqual(reference.Id, ((DBRef)insertedDocument["Reference"]).Id);
        }

        public class Entity
        {
            public Guid Id { get; private set; }

            public EntityRef Reference { get; set; }
        }

        public class EntityRef
        {
            public Guid Id { get; private set; }
        }

        public class EntityMap : FluentClass<Entity>
        {
            public EntityMap()
            {
                Id(x => x.Id);
                Map(x => x.Reference).AsReference();
            }
        }

        public class EntityRefMap : FluentClass<EntityRef>
        {
            public EntityRefMap()
            {
                Id(x => x.Id);
            }
        }

    }
}