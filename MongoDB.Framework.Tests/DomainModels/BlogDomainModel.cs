using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping.Fluent;

namespace MongoDB.Framework.DomainModels
{

    public class BlogMap : FluentRootClassMap<Blog>
    {
        public BlogMap()
        {
            Id(x => x.Id);
            Map(x => x.Name).Key("name");
            Map(x => x.Description).Key("desc");

            //HasMany(x => x.Entries);
        }
    }

    public class BlogEntryMap : FluentRootClassMap<BlogEntry>
    {
        public BlogEntryMap()
        {
            Id(x => x.Id);
            //Reference(x => x.Blog);

            Map(x => x.Title);
            Map(x => x.Body);
            Map(x => x.PostDate);

            Collection(x => x.Comments);
        }
    }

    public class BlogEntryCommentMap : FluentNestedClassMap<BlogEntryComment>
    {
        public BlogEntryCommentMap()
        {
            Map(x => x.CommenterEmail);
            Map(x => x.Comment);
            Collection(x => x.Comments);
        }
    }

    public class Blog
    {
        public Guid Id { get; private set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IList<BlogEntry> Entries { get; set; }

        public Blog()
        {
            this.Entries = new List<BlogEntry>();
        }
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

        public IList<BlogEntryComment> Comments { get; private set; }

        public BlogEntryComment()
        {
            this.Comments = new List<BlogEntryComment>();
        }
    }
}