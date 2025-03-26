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


        public int Id { get; set; }
        public int CallsId { get; set; }
        [ForeignKey("CallsId")]
        public virtual Calls Calls { get; set; }
        public int VolunteerId { get; set; }

        [ForeignKey("VolunteerId")]
        public virtual Volunteers Volunteer { get; set; }

        public DateTime TreatmentDateTime { get; set; }

    }
}


