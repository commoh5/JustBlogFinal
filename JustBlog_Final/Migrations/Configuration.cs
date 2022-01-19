namespace JustBlog_Final.Migrations
{
    using JustBlog_Final.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<IdentitySample.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(IdentitySample.Models.ApplicationDbContext context)
        {
            Category category = new Category() { Name = "Entity Framework", Description = "All post in Entity Framework", UrlSlug = "entity framework" };
            Post post = new Post()
            {
                Category = category,
                Title = "Data Annotations - InverseProperty Attribute in EF 6 & EF Core",
                ShortDescription = "The InverseProperty attribute is used when two entities have more than one relationship. To understand the InverseProperty attribute, consider the following example of Course and Teacher entities.",
                Modified = DateTime.Now,
                PostedOn = DateTime.Now,
                PostContent = @"In the above example, the Course and Teacher entities have two one-to-many relationships. A Course can be taught by an online teacher as well as a class-room teacher. In the same way, a Teacher can teach multiple online courses as well as class room courses.

Here,
                EF API cannot determine the other end of the relationship.It will throw the following exception for the above example during migration.
         

         Unable to determine the relationship represented by navigation property 'Course.OnlineTeacher' of type 'Teacher'.Either manually configure the relationship, or ignore this property using the '[NotMapped]' attribute or by using 'EntityTypeBuilder.Ignore' in 'OnModelCreating'.

        However, EF 6 will create the following Courses table with four foreign keys.",
                Published = true,
                UrlSlug = "data annotation inverse property attribule in ef 6",
                ViewCount = 100,
                Tags = new List<Tag>() {
                    new Tag() {
                        Name = "Entity Framework",
                        Description = "Entity Framework",
                        Count = 100,
                        UrlSlug = "entity framework" } ,
                new Tag() {
                        Name = "MVC",
                        Description = "Microsoft MVC",
                        Count = 50,
                        UrlSlug = "mvc" } },
            };

            context.Categories.Add(category);
            context.Postss.Add(post);

            Comment comment = new Comment()
            {
                Post = post,
                Name = "Scott Trinh",
                Email = "tutb@live.com",
                CommentTime = DateTime.Now,
                CommentHeader = "This is sample comment",
                CommentText = @"This is sample comment with 

        multiple lines"
            };

            context.Comments.Add(comment);

            context.SaveChanges();
            base.Seed(context);
        }
    }
}
