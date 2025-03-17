using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reposetory.Entities
{


    public class Calls
    {
        [Key]
        public int Id { get; set; } // מפתח ראשי

        [Required]
        public string Region { get; set; } // אזור

        [Required]
        public double LocationX { get; set; } // קואורדינטה X

        [Required]
        public double LocationY { get; set; } // קואורדינטה Y

        public string ImageUrl { get; set; } // תמונה

        public string Description { get; set; } // תיאור

        [Required]
        public int UrgencyLevel { get; set; } // רמת דחיפות

        public string Status { get; set; } // סטטוס הקריאה

        public List<VolunteerCalls> VolunteerCalls { get; set; } = new List<VolunteerCalls>();
    }

}