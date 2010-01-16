using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Configuration.Fluent.Mapping;
using MongoDB.Driver;

namespace MongoDB.Framework.Queries
{
    public abstract class QueryTestCase : TestCase
    {
        protected override MongoDB.Framework.Configuration.Mapping.IMapModelRegistry MapModelRegistry
        {
            get
            {
                return new FluentMapModelRegistry()
                    .AddMap(new BlogMap())
                    .AddMap(new BlogEntryMap())
                    .AddMap(new BlogEntryCommentMap());
            }
        }

        protected override void BeforeTest()
        {
            using (var mongoSession = this.OpenMongoSession())
            {
                var blog = new Document()
                    .Append("_id", Guid.NewGuid().ToString("N"))
                    .Append("name", "Bob McBob's Blog")
                    .Append("desc", "What about Bob?");

                var entry1 = new Document()
                    .Append("_id", Guid.NewGuid().ToString("N"))
                    .Append("title", "My First Post")
                    .Append("body", "blah blah blah...")
                    .Append("date", new DateTime(2009, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                    .Append("blogRef", new DBRef("blogs", blog["_id"]))
                    .Append("comments", new Document[2] {
                        new Document()
                            .Append("email", "a@abc.com")
                            .Append("body", "boring!!!"),
                        new Document()
                            .Append("email", "b@bdc.com")
                            .Append("body", "hmm!!!")});

                var entry2 = new Document()
                    .Append("_id", Guid.NewGuid().ToString("N"))
                    .Append("title", "My Second Post")
                    .Append("body", "ooga booga")
                    .Append("date", new DateTime(2009, 1, 2, 0, 0, 0, DateTimeKind.Utc))
                    .Append("blogRef", new DBRef("blogs", blog["_id"]));

                var entry3 = new Document()
                    .Append("_id", Guid.NewGuid().ToString("N"))
                    .Append("title", "Third Post")
                    .Append("body", "I finally have something to say.")
                    .Append("date", new DateTime(2009, 1, 3, 0, 0, 0, DateTimeKind.Utc))
                    .Append("blogRef", new DBRef("blogs", blog["_id"]));

                mongoSession.Database.GetCollection("blogs")
                    .Insert(new[] { blog });
                mongoSession.Database.GetCollection("entries")
                    .Insert(new[] { entry1, entry2, entry3 });
            }
        }

        protected override void AfterTest()
        {
            using (var mongoSession = this.OpenMongoSession())
            {
                mongoSession.Database.MetaData.DropCollection("Blog");
                mongoSession.Database.MetaData.DropCollection("BlogEntry");
            }
        }

        public class BlogMap : FluentRootClass<Blog>
        {
            public BlogMap()
            {
                UseCollection("blogs");

                Id(x => x.Id);
                Map(x => x.Name).Key("name");
                Map(x => x.Description).Key("desc");
            }
        }

        public class BlogEntryMap : FluentRootClass<BlogEntry>
        {
            public BlogEntryMap()
            {
                UseCollection("entries");

                Id(x => x.Id);

                Map(x => x.Title).Key("title");
                Map(x => x.Body).Key("body");
                Map(x => x.PostDate).Key("date");
                Collection(x => x.Comments).Key("comments");

                Reference(x => x.Blog).Key("blogRef");
            }
        }

        public class BlogEntryCommentMap : FluentNestedClass<BlogEntryComment>
        {
            public BlogEntryCommentMap()
            {
                Map(x => x.CommenterEmail).Key("email");
                Map(x => x.Comment).Key("body");
                //Map(x => x.Comments).Key("comments");
            }
        }

        public class Blog
        {
            public Guid Id { get; private set; }

            public string Name { get; set; }

            public string Description { get; set; }
        }

        public class BlogEntry
        {
            public Guid Id { get; private set; }

            public Blog Blog { get; set; }

            public string Title { get; set; }

            public string Body { get; set; }

            public DateTime PostDate { get; set; }

            public IList<BlogEntryComment> Comments { get; private set; }

            public BlogEntry()
            {
                this.Comments = new List<BlogEntryComment>();
            }
        }

        public class BlogEntryComment
        {
            public string CommenterEmail { get; set; }

            public string Comment { get; set; }

            //public IList<BlogEntryComment> Comments { get; private set; }

            public BlogEntryComment()
            {
                //this.Comments = new List<BlogEntryComment>();
            }
        }
    }
}
