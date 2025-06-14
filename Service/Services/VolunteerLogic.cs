using AutoMapper;
using Common.Dto;
using Reposetory.Entities;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repository.Entities;
using Mock;

namespace Service.Services
{
    public class VolunteerLogic : IVolunteerLogic
    {
        private readonly Database db;
        private readonly IMapper mapper;

        public VolunteerLogic(Database db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<VolunteersDto> RegisterVolunteerWithLocation(VolunteersDto dto)
        {
            var entity = mapper.Map<Volunteers>(dto);
            db.VolunteersDb.Add(entity);
            await db.SaveChangesAsync();
            return mapper.Map<VolunteersDto>(entity);
        }

        public List<VolunteersDto> GetNearbyVolunteers(double locationX, double locationY)
        {
            var volunteers = db.VolunteersDb
                .Where(v => v.LocationX != null && v.LocationY != null)
                .ToList();

            var nearby = volunteers
                .Where(v => Distance(v.LocationX!.Value, v.LocationY!.Value, locationX, locationY) < 5)
                .ToList();

            return mapper.Map<List<VolunteersDto>>(nearby);
        }

        public List<CallsDto> GetNearbyOpenCalls(double locationX, double locationY)
        {
            var calls = db.CallsDb
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
