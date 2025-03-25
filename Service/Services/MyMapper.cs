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
            CreateMap<CallsDto, Calls>();
            // אפשר גם להוסיף הפיכות אם צריך (אם זה הדדי)
            CreateMap<Calls, CallsDto>();

            CreateMap<VolunteerCalls, VolunteerCallsDto>();
            // אפשר גם להוסיף הפיכות אם צריך (אם זה הדדי)
            CreateMap<VolunteerCallsDto, VolunteerCalls>(); 
            CreateMap<Volunteers, VolunteersDto>();
            // אפשר גם להוסיף הפיכות אם צריך (אם זה הדדי)
            CreateMap<VolunteersDto, Volunteers>();
        }
    }
}
