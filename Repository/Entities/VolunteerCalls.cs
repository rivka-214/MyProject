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
<<<<<<< HEAD

        public int Id { get; set; } // Primary key

        public int CallsId { get; set; } // Foreign key for calls

        [ForeignKey("CallsId")]
        public virtual Calls Calls { get; set; } // Change Volunteers to Calls if Calls is the entity

        public int VolunteerId { get; set; } // Foreign key for volunteer

=======
        public int Id { get; set; }
        public int CallsId { get; set; }
        [ForeignKey("CallsId")]
        public virtual Calls Calls { get; set; }
        public int VolunteerId { get; set; }
>>>>>>> e0db1c2e030d426d2d54f58510fdb1b41cab3ce3
        [ForeignKey("VolunteerId")]
        public virtual Volunteers Volunteer { get; set; }
<<<<<<< HEAD
=======
        public DateTime TreatmentDateTime { get; set; }
>>>>>>> e0db1c2e030d426d2d54f58510fdb1b41cab3ce3
    }
}


