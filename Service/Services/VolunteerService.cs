using AutoMapper;
using Common.Dto;
using Reposetory.Entities;
using Repository.Interfacese;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Service.Services
{
    public class VolunteerService: IService<VolunteersDto>,IVolunteerLogic
    {
        private readonly IRepository<Volunteers> repository;
        private readonly IMapper mapper;
        public VolunteerService(IRepository<Volunteers> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public VolunteersDto AddItem(VolunteersDto item)
        {
            return mapper.Map<Volunteers, VolunteersDto>(repository.AddItem(mapper.Map<VolunteersDto, Volunteers>(item)));
        }

        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
        }

        public List<VolunteersDto> GetAll()
        {
            return mapper.Map<List<Volunteers>, List<VolunteersDto>>(repository.GetAll());
        }

        public VolunteersDto GetById(int id)
        {

            return mapper.Map<Volunteers, VolunteersDto>(repository.GetById(id));

        }
        public void UpdateItem(int id, VolunteersDto item)
        {
            repository.UpdateItem(id, mapper.Map<VolunteersDto, Volunteers>(item));
        }
        public async Task<VolunteersDto> RegisterVolunteerWithLocation(VolunteersDto dto)
        {
            if (!dto.LocationX.HasValue || !dto.LocationY.HasValue)
            {
                var fullAddress = $"{dto.Address} {dto.City}";
                var coords = await ConvertAddressToCoordinates(fullAddress);

                if (coords != null)
                {
                    dto.LocationX = coords.Value.lat;
                    dto.LocationY = coords.Value.lon;
                }
            }

            var entity = mapper.Map<Volunteers>(dto);
            var saved = repository.AddItem(entity);
            return mapper.Map<VolunteersDto>(saved);
        }
        private async Task<(double lat, double lon)?> ConvertAddressToCoordinates(string address)
        {
            using var client = new HttpClient();
            var url = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(address)}";
            client.DefaultRequestHeaders.UserAgent.ParseAdd("RescueApp/1.0");

            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var results = System.Text.Json.JsonSerializer.Deserialize<List<NominatimResult>>(json);

            if (results != null && results.Count > 0)
                return (double.Parse(results[0].lat), double.Parse(results[0].lon));

            return null;
        }

        private class NominatimResult
        {
            public string lat { get; set; }
            public string lon { get; set; }
        }
        public List<VolunteersDto> GetNearbyVolunteers(double locationX, double locationY)
        {
            var all = repository.GetAll()
                .Where(v => v.LocationX.HasValue && v.LocationY.HasValue)
                .ToList();

            var result = all
                .Select(v => new
                {
                    Volunteer = v,
                    Distance = GetDistance(locationX, locationY, v.LocationX!.Value, v.LocationY!.Value)
                })
                .OrderBy(v => v.Distance)
                .Take(20)
                .Select(v => mapper.Map<VolunteersDto>(v.Volunteer))
                .ToList();

            return result;
        }

        private static double GetDistance(double x1, double y1, double x2, double y2)
        {
            var dx = x1 - x2;
            var dy = y1 - y2;
            return Math.Sqrt(dx * dx + dy * dy);
        }


    }
}
