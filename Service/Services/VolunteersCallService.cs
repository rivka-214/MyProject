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
    internal class VolunteersCallService : IService<VolunteerCallsDto>
    {
        private readonly IRepository<VolunteerCalls> repository;
        private readonly IMapper mapper;
        public VolunteersCallService(IRepository<VolunteerCalls> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
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
    }
}
