
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace Common.Dto
{
    public class CallsDto
    {

        public int Id { get; set; } // מפתח ראשי


     


        public double LocationX { get; set; } // קואורדינטה X


        public double LocationY { get; set; } // קואורדינטה Y


        public byte[]? ArrImage { get; set; } // תמונה

      //  public string ImageUrl { get; set; } = string.Empty;  // ערך ברירת מחדל



        public IFormFile? FileImage { get; set; }//

        public string Description { get; set; } // תיאור


        public int UrgencyLevel { get; set; } // רמת דחיפות

        public string Status { get; set; } // סטטוס הקריאה


    }
}
