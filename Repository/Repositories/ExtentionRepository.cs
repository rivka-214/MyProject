using Microsoft.Extensions.DependencyInjection;
using Reposetory.Entities;
using Repository.Interfacese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public static class ExtentionRepository
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Calls>, CallsRepository>();
            services.AddScoped<IRepository<Volunteers>, VolunteersRepository>();
            services.AddScoped<IRepository<VolunteerCalls>, VolunteersCallsRepository>();
            return services;

        }
    }
}
