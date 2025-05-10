using Common.Dto;
using System.ComponentModel.DataAnnotations;

public class VolunteerCallsDto
{
    [Key]
    public int Id { get; set; }

    public int CallsId { get; set; }

    public int VolunteerId { get; set; }

    public VolunteersDto? Volunteer { get; set; } // Nullable → לא חובה לשלוח
}
