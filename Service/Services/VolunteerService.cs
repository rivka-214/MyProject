


using AutoMapper;
using Common.Dto;
using Reposetory.Entities;
using Repository.Interfacese;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Service.Services
{
    public class VolunteerService : IService<VolunteersDto>
    {
        private readonly IRepository<Volunteers> repository;
        private readonly IMapper mapper;

        public VolunteerService(IRepository<Volunteers> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<VolunteersDto> AddItemAsync(VolunteersDto item)
        {
            var entity = mapper.Map<Volunteers>(item);
            var added = await repository.AddItem(entity);
            return mapper.Map<VolunteersDto>(added);
        }

        public async Task DeleteItemAsync(int id)
        {
            await repository.DeleteItem(id);
        }

        public async Task<List<VolunteersDto>> GetAllAsync()
        {
            var list = await repository.GetAll();
            return mapper.Map<List<VolunteersDto>>(list);
        }

        public async Task<VolunteersDto> GetByIdAsync(int id)
        {
            var entity = await repository.GetById(id);
            return mapper.Map<VolunteersDto>(entity);
        }

        public async Task UpdateItemAsync(int id, VolunteersDto item)
        {
            var entity = mapper.Map<Volunteers>(item);
            await repository.UpdateItem(id, entity);
        }
    }
}



