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
    public class CallService : IService<CallsDto>
    {
        private readonly IRepository<Volunteers> repository;
        private readonly IMapper mapper;
        public CallService(IRepository<Volunteers> repository,IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public CallsDto AddItem(CallsDto item)
        {
          return mapper.Map<Volunteers,CallsDto>(repository.AddItem(mapper.Map<CallsDto,Volunteers>(item )));
         
        }

        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
        }

        public List<CallsDto> GetAll()
        {
            return mapper.Map<List<Volunteers>,List<CallsDto>>(repository.GetAll());
        }

        public CallsDto GetById(int id)
        {
            return mapper.Map<Volunteers, CallsDto>(repository.GetById(id));
        }

        public void UpdateItem(int id, CallsDto item)
        {
             repository.UpdateItem(id, mapper.Map<CallsDto, Volunteers>(item));
        }
    }
}
