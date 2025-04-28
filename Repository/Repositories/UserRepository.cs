using Reposetory.Entities;
using Repository.Entities;
using Repository.Interfacese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    internal class UserRepository : IRepository<User>

    {
        private readonly IContext context;
        public UserRepository(IContext context)
        {
            this.context = context;
        }
        public User AddItem(User item)
        {
            this.context.UsersDb.Add(item);
            this.context.Save();
            return item;
        }

        public void DeleteItem(int id)
        {

            this.context.UsersDb.Remove(GetById(id));
            this.context.Save();
        }

        public List<User> GetAll()
        {
            return this.context.UsersDb.ToList();
        }

        public User GetById(int id)
        {
            return context.UsersDb.FirstOrDefault(x =>x.Id ==id);
        }

        public void UpdateItem(int id, User item)
        {
            var user = GetById(id);

            user.Id=item.Id;
            context.Save();
        }



    

        

    
      
        

      
    }
}
