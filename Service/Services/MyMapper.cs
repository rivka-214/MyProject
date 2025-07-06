using AutoMapper;
using Common.Dto;
using Reposetory.Entities;
using Repository.Entities;

public class MyMapper : Profile
{
    public MyMapper()
    {
        string path = Path.Combine(Environment.CurrentDirectory, "Images/");

        CreateMap<CallsDto, Calls>()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.FileImage.FileName));

        CreateMap<Calls, CallsDto>()
            .ForMember(dest => dest.ArrImage, opt => opt.MapFrom(src =>
                File.Exists(Path.Combine(path, src.ImageUrl))
                ? File.ReadAllBytes(Path.Combine(path, src.ImageUrl))
                : null
            ));

        CreateMap<VolunteerCalls, VolunteerCallsDto>()
            .ForMember(dest => dest.Call, opt => opt.MapFrom(src => src.Calls))
            .ForMember(dest => dest.Volunteer, opt => opt.MapFrom(src => src.Volunteer))
            .ReverseMap()
            .ForMember(dest => dest.Calls, opt => opt.MapFrom(src => src.Call))
            .ForMember(dest => dest.Volunteer, opt => opt.MapFrom(src => src.Volunteer));

        CreateMap<Calls, CallsDto>().ReverseMap();
        CreateMap<Volunteers, VolunteersDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
    }
}
