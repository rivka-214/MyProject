using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class VolunteersDto
    {
        
        public int Id { get; set; } // מפתח ראשי

   //     public string Password { get; set; } // סיסמא

        public string FullName { get; set; } // שם מלא

        public string Password { get; set; } // סיסמא
        public string Gmail { get; set; } // מייל
        public string PhoneNumber { get; set; } // מספר טלפון

        public string Specialization { get; set; } // תפקיד (חובש, מגיש עזרה ראשונה וכו')


      public string Address { get; set; }
        public string City { get; set; }


    }
}
