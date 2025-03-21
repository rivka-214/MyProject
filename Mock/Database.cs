
using Microsoft.EntityFrameworkCore;
using Reposetory.Entities;
using Repository.Interfacese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Mock
{
    public class Database :DbContext,IContext 
    {
        public DbSet<Calls> Calls { get ; set; }
        public DbSet<Volunteers> Volunteers { get; set ; }
        public DbSet<VolunteerCalls> VolunteerCalls { get ; set ; }

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
