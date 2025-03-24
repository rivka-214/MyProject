using AutoMapper;
using Common.Dto;
using Reposetory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class MyMapper : Profile
    {
        public MyMapper()
            
        {
<<<<<<< HEAD
            string path=Path.Combine(Environment.CurrentDirectory,"Images/");//פונקציה שמביאה לי את הניתוב 

            //string to byte[]
            CreateMap<Calls, CallsDto>().ForMember("ArrImage", x => x.MapFrom(y => File.ReadAllBytes(path+y.ImageUrl)));
            CreateMap<CallsDto,Calls >().ForMember("ImageUrl", x => x.MapFrom(y => y.FileImage.FileName));
            //לממש לכל המחלקות למשתנים הבעייתים שצריכים המררה
=======
            CreateMap<CallsDto, Calls>();
            // אפשר גם להוסיף הפיכות אם צריך (אם זה הדדי)
            CreateMap<Calls, CallsDto>();

            CreateMap<VolunteerCalls, VolunteerCallsDto>();
            // אפשר גם להוסיף הפיכות אם צריך (אם זה הדדי)
            CreateMap<VolunteerCallsDto, VolunteerCalls>(); 
            CreateMap<Volunteers, VolunteersDto>();
            // אפשר גם להוסיף הפיכות אם צריך (אם זה הדדי)
            CreateMap<VolunteersDto, Volunteers>();
>>>>>>> e0db1c2e030d426d2d54f58510fdb1b41cab3ce3
        }
    }
}
