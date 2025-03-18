using Reposetory.Entities;
using Repository.Interfacese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class VolunteersRepository : IRepository<Volunteers>
    {
        private readonly IContext context;
        public VolunteersRepository(IContext context)
        {
            this.context = context;
        }

        public Volunteers AddItem(Volunteers item)
        {
            this.context.Volunteers.Add(item);
            this.context.Save();
            return item;
        }
        public void DeleteItem(int id)
        {
            this.context.Volunteers.Remove(GetById(id));
            this.context.Save();
        }

        public List<Volunteers> GetAll()
        {
            return this.context.Volunteers.ToList();
        }

        public Volunteers GetById(int id)
        {
            return context.Volunteers.FirstOrDefault(x => x.Id == id);
        }

        public void UpdateItem(int id, Volunteers item)
        {
            var Volunteer = GetById(id);
            Volunteer.FullName = item.FullName;
            context.Save();
        }







    }
}
