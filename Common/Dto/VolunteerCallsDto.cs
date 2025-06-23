using Common.Dto;
using System.ComponentModel.DataAnnotations;

public class VolunteerCallsDto
{
    public int CallsId { get; set; }
    public int VolunteerId { get; set; }
    public string? VolunteerStatus { get; set; } // "notified", "going", "cant", "arrived", "finished"
    public DateTime? ResponseTime { get; set; }
   
    // ✅ הוספתי את פרטי הקריאה
    public CallsDto? Call { get; set; } // פרטי הקריאה
    public VolunteersDto? Volunteer { get; set; } // פרטי המתנדב
}
