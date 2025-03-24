using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Common.Dto
{
    public class CallsDto
    {
        [Key]
        public int Id { get; set; } // מפתח ראשי

        [Required]
        public string Region { get; set; } // אזור

        [Required]
        public double LocationX { get; set; } // קואורדינטה X

        [Required]
        public double LocationY { get; set; } // קואורדינטה Y

<<<<<<< HEAD
        public byte[]? ArrImage { get; set; } // תמונה
=======
        public byte[] ArrImage { get; set; } // תמונה
>>>>>>> e0db1c2e030d426d2d54f58510fdb1b41cab3ce3
        public IFormFile? FileImage { get; set; }//

        public string Description { get; set; } // תיאור


        public int UrgencyLevel { get; set; } // רמת דחיפות

        public  string Status { get; set; } // סטטוס הקריאה

        public List<VolunteerCallsDto> VolunteerCalls { get; set; } = new List<VolunteerCallsDto>();
    }
}
