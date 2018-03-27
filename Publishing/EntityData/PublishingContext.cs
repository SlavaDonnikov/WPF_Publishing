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
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Publication> Publications { get; set; }
    }
}
// https://www.youtube.com/watch?v=hg3H_pAzoPI // How to create Delete\Update\Save buttons and all code-behind!
