using AutoMapper;
using Common.Dto;
using Microsoft.EntityFrameworkCore;
using Reposetory.Entities;
using Repository.Entities;
using Repository.Interfacese;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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

            // אם לא נשלחו קואורדינטות תקינות - נחשב לפי כתובת
            if (entity.LocationX == null || entity.LocationX == 0 || entity.LocationY == null || entity.LocationY == 0)
            {
                var address = $"{entity.Address}, {entity.City}";
                var (lat, lng) = await GetCoordinatesFromAddress(address);
                entity.LocationX = lat;
                entity.LocationY = lng;
            }

            volunteerRepo.AddItem(entity);
            return mapper.Map<VolunteersDto>(entity);
        }


        public List<VolunteersDto> GetNearbyVolunteers(double locationX, double locationY)
        {
            var volunteers = volunteerRepo.GetAll()
                .Where(v => v.LocationX.HasValue && v.LocationY.HasValue)
                .ToList();

            var volunteersWithDistance = volunteers
                .Select(v => new
                {
                    Volunteer = v,
                    Distance = Distance(v.LocationX!.Value, v.LocationY!.Value, locationX, locationY)
                })
                .OrderBy(x => x.Distance)
                .Take(20)
                .Select(x => x.Volunteer)
                .ToList();

            return mapper.Map<List<VolunteersDto>>(volunteersWithDistance);
        }


        public List<CallsDto> GetNearbyOpenCalls(double locationX, double locationY)
        {
            var calls = callsRepo.GetAll()
                .Where(c => c.Status == "נפתח")
                .ToList();

            var callsWithDistance = calls
                .Select(c => new
                {
                    Call = c,
                    Distance = Distance(c.LocationX, c.LocationY, locationX, locationY)
                })
                .OrderBy(x => x.Distance)
                .Take(20)
                .Select(x => x.Call)
                .ToList();

            return mapper.Map<List<CallsDto>>(callsWithDistance);
        }


        private double Distance(double x1, double y1, double x2, double y2)
        {
            var dx = x1 - x2;
            var dy = y1 - y2;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        private async Task<(double lat, double lng)> GetCoordinatesFromAddress(string address)
        {
            var client = new HttpClient();
            var url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(address)}&format=json&limit=1";

            client.DefaultRequestHeaders.Add("User-Agent", "VolunteerApp");

            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<NominatimResult>>(json);

            if (result != null && result.Any())
            {
                var location = result.First();
                return (double.Parse(location.lat), double.Parse(location.lon));
            }

            return (0, 0); // או אולי תזרוק חריגה או תכתוב לוג
        }

        private class NominatimResult
        {
            public string lat { get; set; }
            public string lon { get; set; }
        }

        public List<CallsDto> GetCallsByStatus(string status)
        {
            var calls = callsRepo.GetAll()
                .Where(c => c.Status == status)
                .ToList();

            return mapper.Map<List<CallsDto>>(calls);
        }


    }
}
