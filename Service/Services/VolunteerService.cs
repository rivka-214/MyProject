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
    public class VolunteerService: IService<VolunteersDto>
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
    }
}
