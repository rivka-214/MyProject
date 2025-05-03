using AutoMapper;
using Common.Dto;
using Reposetory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Repository.Entities;


namespace Service.Services
{
    public class MyMapper : Profile
    {
        public MyMapper()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Images/");//פונקציה שמביאה לי את הניתוב
                                                                                //string to byte[]
            CreateMap<Calls, CallsDto>().ForMember("ArrImage", x => x.MapFrom(y => File.ReadAllBytes(path + y.ImageUrl)));
           CreateMap<CallsDto, Calls>().ForMember("ImageUrl", x => x.MapFrom(y => y.FileImage.FileName));
            
            //CreateMap<CallsDto, Calls>();
            //CreateMap<Calls, CallsDto>();

            CreateMap<VolunteerCalls, VolunteerCallsDto>();        
            CreateMap<VolunteerCallsDto, VolunteerCalls>(); 

            CreateMap<Volunteers, VolunteersDto>();      
            CreateMap<VolunteersDto, Volunteers>(); 

            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
