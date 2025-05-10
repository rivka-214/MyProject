using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Reposetory.Entities.Volunteers;

namespace Reposetory.Entities
{
    public class Volunteers
    {
        [Key]
        public int Id { get; set; } // מפתח ראשי


        public string FullName { get; set; } // שם מלא


        public string PhoneNumber { get; set; } // מספר טלפון

        public string Specialization { get; set; } // תפקיד (חובש, מגיש עזרה ראשונה וכו')

  
        public string Region { get; set; }
        public string City { get; set; } 



        public List<VolunteerCalls> VolunteerCalls { get; set; } = new List<VolunteerCalls>();
    }
}
