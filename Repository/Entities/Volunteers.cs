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

        [Required]
        public string FullName { get; set; } // שם מלא

        [Required]
        public string PhoneNumber { get; set; } // מספר טלפון

        public string Role { get; set; } // תפקיד (חובש, מגיש עזרה ראשונה וכו')

        public bool IsManager { get; set; } // האם מתנדב הוא מנהל

        public List<VolunteerCalls> VolunteerCalls { get; set; } = new List<VolunteerCalls>();
    }
}
