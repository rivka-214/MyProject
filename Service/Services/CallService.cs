
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
using Microsoft.AspNetCore.Http;

namespace Service.Services
{
    public class CallService : IService<CallsDto>, ICallService
    {
        private readonly IRepository<Calls> repository;
        private readonly IMapper mapper;
        private readonly IVolunteersCallLogic _volunteerCallLogic;
        public CallService(IRepository<Calls> repository, IMapper mapper, IVolunteersCallLogic volunteerCallLogic)
        {
            this.repository = repository;
            this.mapper = mapper;
            _volunteerCallLogic = volunteerCallLogic;
        }
        public async Task<CallsDto> AddItemAsync(CallsDto item)
        {
            var entity = mapper.Map<Calls>(item);
            var added = await repository.AddItem(entity);
            return mapper.Map<CallsDto>(added);
        }
        public async Task DeleteItemAsync(int id)
        {
            var call = await repository.GetById(id);
            if (call == null)
                throw new System.Exception("קריאה לא נמצאה");

            await repository.DeleteItem(id);
        }

        public async Task<List<CallsDto>> GetAllAsync()
        {
            var list = await repository.GetAll();
            return mapper.Map<List<CallsDto>>(list);
        }

        public async Task<CallsDto> GetByIdAsync(int id)
        {
            var call = await repository.GetById(id);
            return call == null ? null : mapper.Map<CallsDto>(call);
        }
        public async Task UpdateItemAsync(int id, CallsDto item)
        {
            var call = await repository.GetById(id);
            if (call == null)
                throw new System.Exception("קריאה לא נמצאה");

            var updated = mapper.Map<Calls>(item);
            await repository.UpdateItem(id, updated);
        }
        public async Task<string> GetStatus(int id)
        {
            var call = await repository.GetById(id);
            return call?.Status ?? "לא ידוע";
        }
        public async Task UpdateStatus(int id, string status)
        {
            var call = await repository.GetById(id);
            if (call == null)
                throw new System.Exception("קריאה לא נמצאה");

            call.Status = status;
            await repository.UpdateItem(id, call);
        }
        public async Task CompleteCall(int id, CompleteCallDto dto, int volunteerId)
        {
            var call = await repository.GetById(id);
            if (call == null)
                throw new System.Exception("קריאה לא נמצאה");

            var volunteerStatus = await _volunteerCallLogic.GetVolunteerStatus(id, volunteerId);
            if (volunteerStatus != "arrived")
                throw new System.Exception("רק מתנדב שהגיע יכול לסגור קריאה");

            call.Summary = dto.Summary;
            call.Status = "Closed";
            await repository.UpdateItem(id, call);
        }
        public async Task<string> GetCallStatusWithVolunteersInfo(int id)
        {
            var call = await GetByIdAsync(id);
            if (call == null)
                throw new System.Exception("קריאה לא נמצאה");

            var volunteersInfo = await _volunteerCallLogic.GetCallVolunteersInfo(id);
            return $"סטטוס: {call.Status}, מידע מתנדבים: {volunteersInfo.StatusMessage}";
        }
        public async Task<CallsDto> AddCallAsync(CallsDto call, IFormFile file)
        {
            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Images", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                call.ArrImage = await File.ReadAllBytesAsync(path); // שמירת התמונה כ-byte[]
            }

            call.Status = "Open";
            var entity = mapper.Map<Calls>(call);
            var added = await repository.AddItem(entity);
            var savedCall = mapper.Map<CallsDto>(added);

            if (call.LocationX != 0 && call.LocationY != 0)
            {
                await _volunteerCallLogic.AssignNearbyVolunteersToCall(savedCall.Id, call.LocationX, call.LocationY);
              
            }

            return savedCall;
        }

        public async Task AssignNearbyVolunteersToCall(int callId, double locationX, double locationY)
        {
            await _volunteerCallLogic.AssignNearbyVolunteersToCall(callId, locationX, locationY);
        }
    }
}





