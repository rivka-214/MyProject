using AutoMapper;
using Common.Dto;
using Reposetory.Entities;
using Repository.Entities;
using Repository.Interfacese;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class VolunteerLogic : IVolunteerLogic
    {
        private readonly IRepository<Volunteers> volunteerRepo;
        private readonly IRepository<Calls> callsRepo;
        private readonly IMapper mapper;

        public VolunteerLogic(
            IRepository<Volunteers> volunteerRepo,
            IRepository<Calls> callsRepo,
            IMapper mapper)
        {
            this.volunteerRepo = volunteerRepo;
            this.callsRepo = callsRepo;
            this.mapper = mapper;
        }

        public async Task<VolunteersDto> RegisterVolunteerWithLocation(VolunteersDto dto)
        {
            var entity = mapper.Map<Volunteers>(dto);
            volunteerRepo.AddItem(entity);
            // בהנחה שה-AddItem שומר מיד
            return mapper.Map<VolunteersDto>(entity);
        }

        public List<VolunteersDto> GetNearbyVolunteers(double locationX, double locationY)
        {
            var volunteers = volunteerRepo.GetAll()
                .Where(v => v.LocationX.HasValue && v.LocationY.HasValue)
                .ToList();

            var nearby = volunteers
                .Where(v => Distance(v.LocationX!.Value, v.LocationY!.Value, locationX, locationY) < 5)
                .ToList();

            return mapper.Map<List<VolunteersDto>>(nearby);
        }

        public List<CallsDto> GetNearbyOpenCalls(double locationX, double locationY)
        {
            var calls = callsRepo.GetAll()
                .Where(c => c.Status == "נפתח")
                .ToList();

            var nearby = calls
                .Where(c => Distance(c.LocationX, c.LocationY, locationX, locationY) < 5)
                .ToList();

            return mapper.Map<List<CallsDto>>(nearby);
        }

        private double Distance(double x1, double y1, double x2, double y2)
        {
            var dx = x1 - x2;
            var dy = y1 - y2;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
