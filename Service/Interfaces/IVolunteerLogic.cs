using Common.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IVolunteerLogic
    {
        Task<VolunteersDto> RegisterVolunteerWithLocation(VolunteersDto dto);
        Task<List<VolunteersDto>> GetNearbyVolunteers(double locationX, double locationY);
        Task<List<CallsDto>> GetNearbyOpenCalls(double locationX, double locationY);
        Task<List<CallsDto>> GetCallsByStatus(string status);
    }
}
