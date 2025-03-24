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
        public int Id { get; set; }
        public string? Region { get; set; }
        public double LocationX { get; set; }
        public double LocationY { get; set; }
        public string ImageUrl { get; set; }
        public string? Description { get; set; }
        public int? UrgencyLevel { get; set; }
        public string? Status { get; set; }
        public List<VolunteerCalls> VolunteerCalls { get; set; } = new List<VolunteerCalls>();
        //public string InjuryType { get; set; }
        //public int SeverityLevel { get; set; }
        //public DateTime CallDateTime { get; set; }

    }

    //public class FirstAidInstructions
    //{
    //    [Key]
    //    public int Id { get; set; }

    //    [Required]
    //    public string InjuryType { get; set; }

    //    [Required]
    //    public string InstructionText { get; set; }
    //}
}