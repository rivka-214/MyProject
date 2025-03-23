
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

     
        public DbSet<Calls> CallsDb { get; set; }
        public DbSet<Volunteers> VolunteersDb { get; set; }
        public DbSet<VolunteerCalls> VolunteerCallsDb { get; set; }


        public void Save()
        {
            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

           // optionsBuilder.UseSqlServer("server=(localdb)\\MSSQLLocalDB;database=projectShopDb;trusted_connection=true;TrustServerCertificate=True");

            optionsBuilder.UseSqlServer("server=.;database=projectCalls;trusted_connection=true;TrustServerCertificate=True");

        }
    }
}
