using Reposetory.Entities;
using Repository.Interfacese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class CallsRepository : IRepository<Calls>
    {
        private readonly IContext context;
        public CallsRepository(IContext context)
        {
            this.context = context;
        }

        public Calls AddItem(Calls item)
        {
            this.context.CallsDb.Add(item);
            this.context.Save();
            return item;
        }

        public void DeleteItem(int id)
        {
            this.context.CallsDb.Remove(GetById(id));
            this.context.Save();
        }

        public List<Calls> GetAll()
        {
            return this.context.CallsDb.ToList();
        }

        public Calls GetById(int id)
        {
            return context.CallsDb.FirstOrDefault(x => x.Id == id);

        }

        public void UpdateItem(int id, Calls item)
        {
            var call = GetById(id);

            call.Status = item.Status;        
            context.Save();
        }
    }
}
