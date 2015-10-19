using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Project_DMD.Models
{
    public class TempDataContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }

        public System.Data.Entity.DbSet<Project_DMD.Models.AppUser> AppUsers { get; set; }

        public System.Data.Entity.DbSet<Project_DMD.Models.Visit> Visits { get; set; }

    }
}