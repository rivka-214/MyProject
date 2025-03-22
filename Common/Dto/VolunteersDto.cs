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
        [Key]
        public int Id { get; set; } // מפתח ראשי

        [Required]
        public string FullName { get; set; } // שם מלא

        [Required]
        public string PhoneNumber { get; set; } // מספר טלפון

        public string Role { get; set; } // תפקיד (חובש, מגיש עזרה ראשונה וכו')

        public bool IsManager { get; set; } // האם מתנדב הוא מנהל

      
    }
}
