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
            throw new NotImplementedException();
        }

        public Volunteers AddItem(Volunteers volunteers)
        {
            throw new NotImplementedException();
        }

        public void DeleteItem(int id)
        {
            throw new NotImplementedException();
        }

        public List<Calls> GetAll()
        {
            throw new NotImplementedException();
        }

        public Calls GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateItem(int id, Calls item)
        {
            throw new NotImplementedException();
        }

       
    }
}
