using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    internal class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastNAme { get; set; }
        public string PhoneNumber { get; set; }
        public string Gmail { get; set; }
    }
}
