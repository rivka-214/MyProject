using Microsoft.EntityFrameworkCore;
using Reposetory.Entities;

namespace Mock
{
    public class Database : DbContext
    {
        public DbSet<Calls> ProductsTbl { get; set; }
        public DbSet<Volunteers> VolunteersTbl { get; set; }
        public DbSet<VolunteerCalls> VolunteerCallsTbl { get; set; }

        public void Save()
        {
            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=r=(localdb)\\MSSQLLocalDB;database=project;trusted_connection=true;TrustServerCertificate=True");
        }
    }
}
