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

        [ForeignKey("Calls")]
        public int EmergencyCallId { get; set; } // מפתח זר לקריאה
        public Calls EmergencyCall { get; set; }

        [ForeignKey("Volunteers")]
        public int VolunteerId { get; set; } // מפתח זר למתנדב
        public Volunteers Volunteer { get; set; }
    }

}


