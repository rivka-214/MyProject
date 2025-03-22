using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Reposetory.Entities;


namespace Common.Dto
{
    public class VolunteerCallsDto
    {
        private Volunteers volunteer;

        [Key]
        public int Id { get; set; } // מפתח ראשי


        public int CallsId { get; set; } // מפתח זר לקריאה
        [ForeignKey("CallsId")]
        //public Calls calls { get; set; }


        public int VolunteerId { get; set; } // מפתח זר למתנדב
        [ForeignKey("VolunteerId")]
        public Volunteers Volunteer { get => volunteer; set => volunteer = value; }

    }
}