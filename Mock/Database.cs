using Microsoft.EntityFrameworkCore;
using Reposetory.Entities;
using Repository.Entities;
using Repository.Interfacese;

namespace Mock
{
    public class Database : DbContext, IContext
    {
        public DbSet<Calls> CallsDb { get; set; }
        public DbSet<Volunteers> VolunteersDb { get; set; }
        public DbSet<VolunteerCalls> VolunteerCallsDb { get; set; }
        public DbSet<User> UsersDb { get; set; }

        public void Save()
        {
            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=CitizenShield4;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}
