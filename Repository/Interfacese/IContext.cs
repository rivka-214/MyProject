using Microsoft.EntityFrameworkCore;
using Reposetory.Entities;
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
        public DbSet<Calls> Calls { get; set; }
        public DbSet<Volunteers> Volunteers { get; set; }
        public DbSet<VolunteerCalls> VolunteerCalls { get; set; }
        public void Save();
    }
}
