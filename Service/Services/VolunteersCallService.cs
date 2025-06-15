using AutoMapper;
using Common.Dto;
using Reposetory.Entities;
using Repository.Interfacese;
using Service.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class VolunteersCallService : IService<VolunteerCallsDto>, IVolunteersCallLogic
    {
        private readonly IRepository<VolunteerCalls> repository;
        private readonly IMapper mapper;
        private readonly IVolunteerLogic volunteerLogic;

        public VolunteersCallService(
            IRepository<VolunteerCalls> repository,
            IMapper mapper,
            IVolunteerLogic volunteerLogic)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.volunteerLogic = volunteerLogic;
        }

        public VolunteerCallsDto AddItem(VolunteerCallsDto item)
        {
            var entity = mapper.Map<VolunteerCalls>(item);
            var added = repository.AddItem(entity);
            return mapper.Map<VolunteerCallsDto>(added);
        }

        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
        }

        public List<VolunteerCallsDto> GetAll()
        {
            var list = repository.GetAll();
            return mapper.Map<List<VolunteerCallsDto>>(list);
        }

        public VolunteerCallsDto GetById(int id)
        {
            var item = repository.GetById(id);
            return mapper.Map<VolunteerCallsDto>(item);
        }

        public void UpdateItem(int id, VolunteerCallsDto item)
        {
            var entity = mapper.Map<VolunteerCalls>(item);
            repository.UpdateItem(id, entity);
        }

        public async Task AssignNearbyVolunteersToCall(int callId, double locationX, double locationY)
        {
            var nearbyVolunteers = volunteerLogic.GetNearbyVolunteers(locationX, locationY);

            foreach (var volunteer in nearbyVolunteers)
            {
                var newItem = new VolunteerCallsDto
                {
                    CallsId = callId,
                    VolunteerId = volunteer.Id
                    // אפשר להוסיף גם TreatmentDateTime אם צריך
                };

                var entity = mapper.Map<VolunteerCalls>(newItem);
                repository.AddItem(entity);
            }
        }
    }
}
