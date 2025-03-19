using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reposetory.Entities
{
    public class VolunteerCalls
    {
        [Key]
        public int Id { get; set; } // מפתח ראשי

        
        public int CallsId { get; set; } // מפתח זר לקריאה
        [ForeignKey("CallsId")]
        public Volunteers calls { get; set; }

       
        public int VolunteerId { get; set; } // מפתח זר למתנדב
        [ForeignKey("VolunteerId")]
        public Volunteers Volunteer { get; set; }
    }

}


