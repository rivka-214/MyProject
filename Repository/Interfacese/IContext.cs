using Microsoft.EntityFrameworkCore;
using Reposetory.Entities;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfacese
{
    public interface IContext
    {
        public DbSet<Calls> CallsDb { get; set; }
        public DbSet<Volunteers> VolunteersDb { get; set; }
        public DbSet<VolunteerCalls> VolunteerCallsDb { get; set; }
        public DbSet<User> UsersDb { get; set; }
        public void Save();
    }
}
