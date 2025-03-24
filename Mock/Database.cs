
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
    public class Database : DbContext, IContext
    {
<<<<<<< HEAD
=======

     
>>>>>>> e0db1c2e030d426d2d54f58510fdb1b41cab3ce3
        public DbSet<Calls> CallsDb { get; set; }
        public DbSet<Volunteers> VolunteersDb { get; set; }
        public DbSet<VolunteerCalls> VolunteerCallsDb { get; set; }

<<<<<<< HEAD
=======

>>>>>>> e0db1c2e030d426d2d54f58510fdb1b41cab3ce3
        public void Save()
        {
            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
<<<<<<< HEAD
            optionsBuilder.UseSqlServer("server=.;database=projectCalls;trusted_connection=true;TrustServerCertificate=True");
=======


            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS01;Database=CitizenShield;Trusted_Connection=True;TrustServerCertificate=True;\r\n");

>>>>>>> e0db1c2e030d426d2d54f58510fdb1b41cab3ce3
        }
    }
}
