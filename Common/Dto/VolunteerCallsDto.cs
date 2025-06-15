using Common.Dto;
using System.ComponentModel.DataAnnotations;

public class VolunteerCallsDto
{
    public int CallsId { get; set; }
    public int VolunteerId { get; set; }
    public DateTime? TreatmentDateTime { get; set; } // כדאי להוסיף אם תרצי תיעוד עתידי

}
