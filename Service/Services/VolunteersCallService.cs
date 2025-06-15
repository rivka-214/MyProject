using AutoMapper;
using Common.Dto;
using Reposetory.Entities;
using Repository.Interfacese;
using Repository.Repositories;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class VolunteersCallService : IService<VolunteerCallsDto>, IVolunteersCallLogic
    {
        private readonly IRepository<VolunteerCalls> repository;
        private readonly IMapper mapper;
        private readonly IContext context;

        public VolunteersCallService(IRepository<VolunteerCalls> repository, IMapper mapper, IContext context)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.context = context;
        }

        public VolunteerCallsDto AddItem(VolunteerCallsDto item)
        {
            return mapper.Map<VolunteerCalls, VolunteerCallsDto>(repository.AddItem(mapper.Map<VolunteerCallsDto, VolunteerCalls>(item)));

        }

        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
        }

        public List<VolunteerCallsDto> GetAll()
        {
            return mapper.Map<List<VolunteerCalls>, List<VolunteerCallsDto>>(repository.GetAll());

        }

        public VolunteerCallsDto GetById(int id)
        {
            return mapper.Map<VolunteerCalls, VolunteerCallsDto>(repository.GetById(id));

        }

        public void UpdateItem(int id, VolunteerCallsDto item)
        {
            repository.UpdateItem(id, mapper.Map<VolunteerCallsDto, VolunteerCalls>(item));
        }
        public async Task AssignNearbyVolunteersToCall(int callId, double locationX, double locationY)
        {
            var volunteerService = new VolunteerService(new VolunteersRepository(context), mapper);

            var nearbyVolunteers = volunteerService.GetNearbyVolunteers(locationX, locationY);

            foreach (var volunteer in nearbyVolunteers)
            {
                var newItem = new VolunteerCallsDto
                {
                    CallsId = callId,
                    VolunteerId = volunteer.Id,
                    // TreatmentDateTime = DateTime.Now ← נוסיף אחר כך בלחיצה על כפתור
                };

                repository.AddItem(mapper.Map<VolunteerCallsDto, VolunteerCalls>(newItem));
            }
        }


    }
}
