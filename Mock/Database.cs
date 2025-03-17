<<<<<<< HEAD
﻿using Microsoft.EntityFrameworkCore;
using Reposetory.Entities;

namespace Mock
{
    public class Database : DbContext
    {
        public DbSet<Calls> ProductsTbl { get; set; }
        public DbSet<Volunteers> VolunteersTbl { get; set; }
        public DbSet<VolunteerCalls> VolunteerCallsTbl { get; set; }
=======
﻿
using Reposetory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Mock
{
    public class Database :DbContext ,IContext
    {
        public DbSet<Calls> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
>>>>>>> 99e43d69f7efca8085951ac4b5eff6425d61b2dd

        public void Save()
        {
            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
<<<<<<< HEAD
            optionsBuilder.UseSqlServer("server=r=(localdb)\\MSSQLLocalDB;database=project;trusted_connection=true;TrustServerCertificate=True");
=======
            optionsBuilder.UseSqlServer("server=sql;database=projectShopDb;trusted_connection=true;TrustServerCertificate=True");
>>>>>>> 99e43d69f7efca8085951ac4b5eff6425d61b2dd
        }
    }
}
