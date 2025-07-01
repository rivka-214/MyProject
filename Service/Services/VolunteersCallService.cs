using AutoMapper;
using Common.Dto;
using Reposetory.Entities;
using Repository.Entities;
using Repository.Interfacese;
using Repository.Repositories;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Service.Services
{
    public class VolunteersCallService : IService<VolunteerCallsDto>, IVolunteersCallLogic
    {
        private readonly IRepository<VolunteerCalls> _repository;
        private readonly IRepository<Calls> _callsRepository; // החלפה של ICallService
        private readonly IMapper _mapper;
        private readonly IVolunteerLogic _volunteerLogic;
      


        public VolunteersCallService(
            IRepository<VolunteerCalls> repository,
            IRepository<Calls> callsRepository, // הוספה
            IMapper mapper,
            IVolunteerLogic volunteerLogic)
        {
            _repository = repository;
            _callsRepository = callsRepository;
            _mapper = mapper;
            _volunteerLogic = volunteerLogic;
         
         
           
        }

        public async Task<VolunteerCallsDto> AddItemAsync(VolunteerCallsDto item)
        {
            var entity = _mapper.Map<VolunteerCalls>(item);
            var added = await _repository.AddItem(entity);
            return _mapper.Map<VolunteerCallsDto>(added);
        }

        public async Task DeleteItemAsync(int id)
        {
            var call = await _repository.GetById(id);
            if (call == null)
                throw new System.Exception("קריאה לא נמצאה");

            await _repository.DeleteItem(id);
        }

        public async Task<List<VolunteerCallsDto>> GetAllAsync()
        {
            var list = await _repository.GetAll();
            return _mapper.Map<List<VolunteerCallsDto>>(list);
        }

        public async Task<VolunteerCallsDto> GetByIdAsync(int id)
        {
            var item = await _repository.GetById(id);
            return _mapper.Map<VolunteerCallsDto>(item);
        }

        public async Task UpdateItemAsync(int id, VolunteerCallsDto item)
        {
            var call = await _repository.GetById(id);
            if (call == null)
                throw new System.Exception("קריאה לא נמצאה");

            var entity = _mapper.Map<VolunteerCalls>(item);
            await _repository.UpdateItem(id, entity);
        }

        public async Task AssignNearbyVolunteersToCall(int callId, double locationX, double locationY)
        {
            var nearbyVolunteers = await _volunteerLogic.GetNearbyVolunteers(locationX, locationY);
            foreach (var volunteer in nearbyVolunteers.Take(20))
            {
                var newItem = new VolunteerCallsDto
                {
                    CallsId = callId,
                    VolunteerId = volunteer.Id,
                    VolunteerStatus = "notified",
                    ResponseTime = DateTime.UtcNow
                };
                await AddItemAsync(newItem);
            }
        }

        public async Task RespondToCall(int callId, int volunteerId, string response, int currentVolunteerId)
        {
            if (volunteerId != currentVolunteerId)
                throw new UnauthorizedAccessException("אין הרשאה לעדכן מתנדב אחר");

            var repo = _repository as VolunteersCallsRepository;
            if (repo == null)
                throw new Exception("שגיאה פנימית במסד הנתונים");

            var exists = await repo.GetVolunteerCall(callId, volunteerId);
            if (exists == null)
                throw new Exception("המתנדב לא משויך לקריאה זו");

            await repo.UpdateVolunteerStatus(callId, volunteerId, response);

            if (response == "going")
            {
                if (await ShouldSendToMoreVolunteers(callId))
                {
                    var call = await _callsRepository.GetById(callId);
                    if (call != null)
                        await AssignNearbyVolunteersToCall(callId, call.LocationX, call.LocationY);
                }
            }
            else if (response == "arrived")
            {
                var call = await _callsRepository.GetById(callId);
                if (call == null)
                    throw new System.Exception("קריאה לא נמצאה");

                call.Status = "InProgress";
                await _callsRepository.UpdateItem(callId, call);
            }
        }

        public async Task UpdateVolunteerStatus(int callId, int volunteerId, string status, int currentVolunteerId, string summary = null)
        {
            if (volunteerId != currentVolunteerId)
                throw new UnauthorizedAccessException("אין הרשאה לעדכן מתנדב אחר");

            var repo = _repository as VolunteersCallsRepository;
            if (repo == null)
                throw new Exception("שגיאה פנימית במסד הנתונים");

            var exists = await repo.GetVolunteerCall(callId, volunteerId);
            if (exists == null)
                throw new Exception("המתנדב לא משויך לקריאה זו");

            await repo.UpdateVolunteerStatus(callId, volunteerId, status);

            if (status == "arrived")
            {
                var call = await _callsRepository.GetById(callId);
                if (call == null)
                    throw new System.Exception("קריאה לא נמצאה");

                call.Status = "InProgress";
                await _callsRepository.UpdateItem(callId, call);
            }
            else if (status == "finished")
            {
                var call = await _callsRepository.GetById(callId);
                if (call == null)
                    throw new System.Exception("קריאה לא נמצאה");

                if (!string.IsNullOrEmpty(summary))
                {
                    call.Summary = summary;
                    call.Status = "Closed";
                    await _callsRepository.UpdateItem(callId, call);
                }
                else
                {
                    call.Status = "Closed";
                    await _callsRepository.UpdateItem(callId, call);
                }
            }
        }

        public async Task<bool> ShouldSendToMoreVolunteers(int callId)
        {
            var repo = _repository as VolunteersCallsRepository;
            if (repo == null)
                return false;

            var goingCount = await repo.GetGoingVolunteersCount(callId);
            var hasArrived = await repo.HasArrivedVolunteer(callId);
            return goingCount < 3 && !hasArrived;
        }

        public async Task<CallVolunteersInfoDto> GetCallVolunteersInfo(int callId)
        {
            var repo = _repository as VolunteersCallsRepository;
            if (repo == null)
                throw new Exception("שגיאה פנימית");

            var goingCount = await repo.GetGoingVolunteersCount(callId);
            var hasArrived = await repo.HasArrivedVolunteer(callId);

            var statusMessage = hasArrived
                ? "מתנדב הגיע למקום - בטיפול"
                : goingCount > 0
                    ? $"{goingCount} מתנדבים יצאו לקריאה"
                    : "ממתין למתנדבים";

            return new CallVolunteersInfoDto
            {
                CallId = callId,
                GoingVolunteersCount = goingCount,
                HasArrivedVolunteer = hasArrived,
                StatusMessage = statusMessage
            };
        }

        public async Task<int> GetGoingVolunteersCount(int callId)
        {
            var repo = _repository as VolunteersCallsRepository;
            return await repo.GetGoingVolunteersCount(callId);
        }

        public async Task<bool> HasArrivedVolunteer(int callId)
        {
            var repo = _repository as VolunteersCallsRepository;
            return await repo.HasArrivedVolunteer(callId);
        }

        public async Task<string> GetVolunteerStatus(int callId, int volunteerId)
        {
            var repo = _repository as VolunteersCallsRepository;
            var call = await repo?.GetVolunteerCall(callId, volunteerId);
            return call?.VolunteerStatus ?? "notified";
        }

        public async Task<VolunteerCallsDto> GetVolunteerCall(int callId, int volunteerId)
        {
            var repo = _repository as VolunteersCallsRepository;
            var call = await repo?.GetVolunteerCall(callId, volunteerId);
            return call == null ? null : _mapper.Map<VolunteerCallsDto>(call);
        }

        public async Task<List<VolunteerCallsDto>> GetActiveCallsForVolunteer(int volunteerId)
        {
            var repo = _repository as VolunteersCallsRepository;
            if (repo == null)
                throw new Exception("שגיאה פנימית במסד הנתונים");

            var activeCalls = await repo.GetActiveCallsForVolunteer(volunteerId);
            return _mapper.Map<List<VolunteerCallsDto>>(activeCalls ?? new List<VolunteerCalls>());
        }

        public async Task<List<VolunteerCallsDto>> GetHistoryCallsForVolunteer(int volunteerId)
        {
            var repo = _repository as VolunteersCallsRepository;
            if (repo == null)
                throw new Exception("שגיאה פנימית במסד הנתונים");

            var historyCalls = await repo.GetHistoryCallsForVolunteer(volunteerId);
            return _mapper.Map<List<VolunteerCallsDto>>(historyCalls ?? new List<VolunteerCalls>());
        }

      
        public async Task CheckAndReassignVolunteers()
        {
            var openCalls = await _callsRepository.GetAll();
            foreach (var call in openCalls.Where(c => c.Status == "Open"))
            {
                if (await ShouldSendToMoreVolunteers(call.Id))
                {
                    await AssignNearbyVolunteersToCall(call.Id, call.LocationX, call.LocationY);
                }
            }
        }

       
    }
}