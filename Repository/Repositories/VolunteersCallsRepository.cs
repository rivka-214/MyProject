using Reposetory.Entities;
using Repository.Interfacese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class VolunteersCallsRepository : IRepository<VolunteerCalls>
    {
        private readonly IContext context;
        public VolunteersCallsRepository(IContext context)
        {
            this.context = context;
        }
   
    

        public VolunteerCalls AddItem(VolunteerCalls item)
        {

            this.context.VolunteerCallsDb.Add(item);
            this.context.Save();
            return item;
        }


        public void DeleteItem(int id)
        {
            this.context.VolunteerCallsDb.Remove(GetById(id));
            this.context.Save();
        }

        public List<VolunteerCalls> GetAll()
        {

            return this.context.VolunteerCallsDb.ToList();
        }

        public VolunteerCalls GetById(int id)
        {
            return context.VolunteerCallsDb.FirstOrDefault(x => x.Id == id); 
        }

        public void UpdateItem(int id, VolunteerCalls item)
        {
            var VolunteerCall = GetById(id);
            VolunteerCall.Id= item.Id;
            context.Save();
        }
    }
}
