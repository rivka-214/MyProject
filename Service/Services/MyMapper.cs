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
                                                                                //     CreateMap<Calls, CallsDto>().ForMember("ArrImage", x => x.MapFrom(y => File.ReadAllBytes(path + y.ImageUrl)));
            CreateMap<CallsDto, Calls>().ForMember("ImageUrl", x => x.MapFrom(y => y.FileImage.FileName));
            CreateMap<Calls, CallsDto>()
            .ForMember("ArrImage", x => x.MapFrom(y =>
            File.Exists(Path.Combine(path, y.ImageUrl))
            ? File.ReadAllBytes(Path.Combine(path, y.ImageUrl))
            : null
    ));

           
            CreateMap<VolunteerCalls, VolunteerCallsDto>()
                .ForMember(dest => dest.Call, opt => opt.MapFrom(src => src.Calls))
                .ReverseMap()
                .ForMember(dest => dest.Calls, opt => opt.MapFrom(src => src.Call));

            
            CreateMap<Volunteers, VolunteersDto>().ReverseMap();
            CreateMap<VolunteerCalls, CallsDto>();
            CreateMap<Calls, CallsDto>();



            CreateMap<User, UserDto>().ReverseMap();
        }
       
    }

}
