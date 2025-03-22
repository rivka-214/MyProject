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

        public int Id { get; set; } // Primary key


        public int CallsId { get; set; } // Foreign key for calls

        [ForeignKey("CallsId")]

        public virtual Calls Calls { get; set; } // Change Volunteers to Calls if Calls is the entity


        public int VolunteerId { get; set; } // Foreign key for volunteer

        [ForeignKey("VolunteerId")]

        public virtual Volunteers Volunteer { get; set; }

    }


}


