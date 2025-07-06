using AutoMapper;
using Common.Dto;
using Reposetory.Entities;
using Repository.Entities;

public class MyMapper : Profile
{
    public MyMapper()
    {
        string path = Path.Combine(Environment.CurrentDirectory, "Images/");

        // CallsDto → Calls
        CreateMap<CallsDto, Calls>()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src =>
                src.FileImage != null ? src.FileImage.FileName : null));

        // Calls → CallsDto
        CreateMap<Calls, CallsDto>()
            .ForMember(dest => dest.ArrImage, opt => opt.MapFrom(src =>
                File.Exists(Path.Combine(path, src.ImageUrl))
                ? File.ReadAllBytes(Path.Combine(path, src.ImageUrl))
                : null));

        // VolunteerCalls ↔ VolunteerCallsDto
        CreateMap<VolunteerCalls, VolunteerCallsDto>()
            .ForMember(dest => dest.Call, opt => opt.MapFrom(src => src.Calls))
            .ForMember(dest => dest.Volunteer, opt => opt.MapFrom(src => src.Volunteer))
            .ReverseMap()
            .ForMember(dest => dest.Calls, opt => opt.MapFrom(src => src.Call))
            .ForMember(dest => dest.Volunteer, opt => opt.MapFrom(src => src.Volunteer));

        // Others
        CreateMap<Volunteers, VolunteersDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
    }
}
