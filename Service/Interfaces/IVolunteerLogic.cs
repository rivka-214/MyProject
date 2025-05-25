using Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IVolunteerLogic
    {
        Task<VolunteersDto> RegisterVolunteerWithLocation(VolunteersDto dto);
        List<VolunteersDto> GetNearbyVolunteers(double locationX, double locationY);

    }

}
