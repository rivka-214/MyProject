
using Microsoft.EntityFrameworkCore;
using Reposetory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Mock
{
    public class Database :DbContext 
    {
        public DbSet<Calls> CallsTbl { get; set; }
        public DbSet<Volunteers> VolunteersTbl { get; set; }
        public DbSet<VolunteerCalls> VolunteerCallsTbl { get; set; }

        public void Save()
        {
            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=sql;database=projectShopDb;trusted_connection=true;TrustServerCertificate=True");
        }
    }
}
