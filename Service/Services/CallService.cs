
using AutoMapper;
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
    public class CallService : IService<CallsDto>, ICallService
    {
        private readonly IRepository<Calls> repository;
        private readonly IMapper mapper;

        public CallService(IRepository<Calls> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<CallsDto> AddItemAsync(CallsDto item)
        {
            var entity = mapper.Map<Calls>(item);
            var added = await repository.AddItem(entity);
            return mapper.Map<CallsDto>(added);
        }

        public async Task DeleteItemAsync(int id)
        {
            await repository.DeleteItem(id);
        }

        public async Task<List<CallsDto>> GetAllAsync()
        {
            var list = await repository.GetAll();
            return mapper.Map<List<CallsDto>>(list);
        }

        public async Task<CallsDto> GetByIdAsync(int id)
        {
            var entity = await repository.GetById(id);
            return mapper.Map<CallsDto>(entity);
        }

        public async Task UpdateItemAsync(int id, CallsDto item)
        {
            var entity = mapper.Map<Calls>(item);
            await repository.UpdateItem(id, entity);
        }

        public async Task<string> GetStatus(int id)
        {
            var call = await repository.GetById(id);
            return call?.Status ?? "לא ידוע";
        }

        public async Task UpdateStatus(int id, string status)
        {
            var call = await repository.GetById(id);
            if (call != null)
            {
                call.Status = status;
                await repository.UpdateItem(id, call);
            }
        }

        public async Task CompleteCall(int id, CompleteCallDto dto)
        {
            var call = await repository.GetById(id);
            if (call == null)
                throw new Exception("קריאה לא קיימת");

            call.Summary = dto.Summary;
            call.SentToHospital = dto.SentToHospital;
            call.HospitalName = dto.SentToHospital ? dto.HospitalName : null;
            await repository.UpdateItem(id, call);
        }
    }
}