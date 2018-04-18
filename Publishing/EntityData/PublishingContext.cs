using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Publishing.EntityData
{
    class PublishingContext : DbContext
    {
        static PublishingContext()
        {
            //Database.SetInitializer<PublishingHouseDB>(new CreateDatabaseIfNotExists<PublishingHouseDB>());
            //Database.SetInitializer<SchoolDBContext>(new DropCreateDatabaseIfModelChanges<SchoolDBContext>());
            //Database.SetInitializer<SchoolDBContext>(new DropCreateDatabaseAlways<SchoolDBContext>());
            //Database.SetInitializer<SchoolDBContext>(new SchoolDBInitializer()); 

            Database.SetInitializer<PublishingContext>(new DBContextInitializer());
            //Database.SetInitializer<PublishingContext>(new DropCreateDatabaseAlways<PublishingContext>());
        }

        public PublishingContext() : base("PublishingHouseDB") // specify the database name
        {

        }

        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Publication> Publications { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    // configures not null one-to-many relationship between Publication and Publisher tables
        //    modelBuilder.Entity<Publication>()
        //        .HasRequired<Publisher>(s => s.Publisher)
        //        .WithMany(g => g.Publications)
        //        .HasForeignKey<int>(s => s.PublisherRefId);
        //}

        // !!! Cascade delete is enabled by default in Entity Framework for all types of relationships such as one-to-one, one-to-many and many-to-many.
    }    
}
// https://www.youtube.com/watch?v=hg3H_pAzoPI // How to create Delete\Update\Save buttons and all code-behind!
