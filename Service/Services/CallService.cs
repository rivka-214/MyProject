﻿using AutoMapper;

using Reposetory.Entities;
using Repository.Interfacese;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;
namespace Service.Services
{
    public class CallService : IService<CallsDto>
    {
        private readonly IRepository<Calls> repository;  
        private readonly IMapper mapper;
        public CallService(IRepository<Calls> repository,IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public CallsDto AddItem(CallsDto item)
        {
          return mapper.Map<Calls, CallsDto>(repository.AddItem(mapper.Map<CallsDto, Calls>(item )));
         
        }
        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
        }
        public List<CallsDto> GetAll()
        {
            return mapper.Map<List<Calls>,List<CallsDto>>(repository.GetAll());
        }
        public CallsDto GetById(int id)
        {
            return mapper.Map<Calls, CallsDto>(repository.GetById(id));
        }
        public void UpdateItem(int id, CallsDto item)
        {
             repository.UpdateItem(id, mapper.Map<CallsDto, Calls>(item));
        }
    }
}
