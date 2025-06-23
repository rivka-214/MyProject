using System;

namespace Common.Dto
{
    public class VolunteerResponseDto
    {
        public int CallId { get; set; }
        public int VolunteerId { get; set; }
        public string Response { get; set; } // "going", "cant", "arrived", "finished"
    }

    public class UpdateVolunteerStatusDto
    {
        public string Status { get; set; } // "going", "cant", "arrived", "finished"
        public string Summary { get; set; } // רק לסטטוס "finished"
    }

    public class CallVolunteersInfoDto
    {
        public int CallId { get; set; }
        public int GoingVolunteersCount { get; set; }
        public bool HasArrivedVolunteer { get; set; }
        public string StatusMessage { get; set; }
    }
}
