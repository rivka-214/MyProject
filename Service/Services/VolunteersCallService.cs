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

namespace Service.Services
{
    public class VolunteersCallService : IService<VolunteerCallsDto>, IVolunteersCallLogic
    {
        private readonly IRepository<VolunteerCalls> _repository;
        private readonly IRepository<Calls> _callsRepository;
        private readonly IMapper _mapper;
        private readonly IVolunteerLogic _volunteerLogic;

        public VolunteersCallService(
            IRepository<VolunteerCalls> repository,
            IRepository<Calls> callsRepository,
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

            // שליפת הקריאה המלאה מה-Repository
            var callEntity = await _callsRepository.GetById(callId);
            var callDetails = _mapper.Map<CallsDto>(callEntity);

            foreach (var volunteer in nearbyVolunteers.Take(20))
            {
                var newItem = new VolunteerCallsDto
                {
                    CallsId = callId,
                    VolunteerId = volunteer.Id,
                    VolunteerStatus = "notified",
                    ResponseTime = DateTime.UtcNow,
                    GoingVolunteersCount = 0
                };

                await AddItemAsync(newItem);
            }
        }




        public async Task UpdateVolunteerStatus(int callId, int volunteerId, string status, int currentVolunteerId)
        {
            Console.WriteLine($"[Logic] callId={callId}, volunteerId={volunteerId}, status={status}, currentVolunteerId={currentVolunteerId}");

            if (string.IsNullOrEmpty(status))
                throw new ArgumentException("status cannot be null or empty", nameof(status));

            var allowed = new[] { "notified", "going", "cant", "arrived" };
            if (!allowed.Contains(status))
                throw new ArgumentException("Invalid status value", nameof(status));

            if (volunteerId != currentVolunteerId)
                throw new UnauthorizedAccessException("אין הרשאה לעדכן מתנדב אחר");

            var repo = _repository as VolunteersCallsRepository;
            if (repo == null)
                throw new Exception("שגיאה פנימית במסד הנתונים");

            var exists = await repo.GetVolunteerCall(callId, volunteerId);
            if (exists == null)
                throw new Exception("המתנדב לא משויך לקריאה זו");

            // 🚫 לא לאפשר arrived לפני going
            if (status == "arrived" && exists.VolunteerStatus != "going")
                throw new InvalidOperationException("לא ניתן לעדכן ל-'arrived' לפני שהסטטוס הוא 'going'");

            await repo.UpdateVolunteerStatus(callId, volunteerId, status);

            if (status == "arrived")
            {
                var call = await _callsRepository.GetById(callId);
                if (call == null)
                    throw new Exception("קריאה לא נמצאה");

                call.Status = "InProgress";
                await _callsRepository.UpdateItem(callId, call);
            }

            if (status == "going")
            {
                var call = await _callsRepository.GetById(callId);
                if (call == null)
                    throw new Exception("קריאה לא נמצאה");

                call.numVolanteer++;
                await _callsRepository.UpdateItem(callId, call);
            }
        }


        public async Task CompleteCallAsync(int callId, int volunteerId, int currentVolunteerId, CompleteCallDto dto)
        {
            if (volunteerId != currentVolunteerId)
                throw new UnauthorizedAccessException("אין הרשאה לעדכן מתנדב אחר");

            await (_repository as VolunteersCallsRepository).CompleteCallAndUpdateVolunteers(
                callId,
                volunteerId,
                dto.Summary,
                dto.SentToHospital,
                dto.HospitalName);
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
        public async Task<List<VolunteerCallsDto>> GetnotifiedCallsForVolunteer(int volunteerId)
        {
            var repo = _repository as VolunteersCallsRepository;
            if (repo == null)
                throw new Exception("שגיאה פנימית במסד הנתונים");

            var notifiedCalls = await repo.GetnotifiedCallsForVolunteer(volunteerId);
            return _mapper.Map<List<VolunteerCallsDto>>(notifiedCalls ?? new List<VolunteerCalls>());
        }


        public async Task<List<VolunteerCallsDto>> GetHistoryCallsForVolunteer(int volunteerId)
        {
            var repo = _repository as VolunteersCallsRepository;
            if (repo == null)
                throw new Exception("שגיאה פנימית במסד הנתונים");

            var historyCalls = await repo.GetHistoryCallsForVolunteer(volunteerId);
            return _mapper.Map<List<VolunteerCallsDto>>(historyCalls ?? new List<VolunteerCalls>());
        }
        /// מחזיר את כל הקריאות שהוקצו למתנדב
        public async Task<List<CallsDto>> GetAllCallsForVolunteer(int volunteerId)
        {
            var repo = _repository as VolunteersCallsRepository;
            if (repo == null)
                throw new Exception("שגיאה פנימית במסד הנתונים");

            // שלוף את כל הקריאות של המתנדב
            var volunteerCalls = await repo.GetAllCallsForVolunteer(volunteerId);

            // הדפס לדיבוג
            Console.WriteLine($"נמצאו {volunteerCalls.Count} קריאות למתנדב {volunteerId}");

            // תמפה ל-CallsDto
            var calls = volunteerCalls
                .Where(vc => vc.Calls != null) // ודא שיש קריאה
                .Select(vc =>
                {
                    Console.WriteLine($"ממפה קריאה ID: {vc.Calls.Id}, תיאור: {vc.Calls.Description}");
                    return _mapper.Map<CallsDto>(vc.Calls);
                })
                .ToList();

            Console.WriteLine($"הוחזרו {calls.Count} קריאות אחרי המיפוי");
            return calls;
        }

        public async Task<List<CallsDto>> GetCallsForVolunteerByStatus(int volunteerId, string status)
        {
            var repo = _repository as VolunteersCallsRepository;
            if (repo == null)
                throw new Exception("שגיאה פנימית במסד הנתונים");

            var calls = await repo.GetCallsForVolunteerByStatus(volunteerId, status);

            return _mapper.Map<List<CallsDto>>(calls);
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

        public async Task<List<VolunteersDto>> GetTop20VolunteersForCall(int callId)
        {
            var call = await _callsRepository.GetById(callId);
            if (call == null)
                throw new Exception("קריאה לא נמצאה");

            var nearbyVolunteers = await _volunteerLogic.GetNearbyVolunteers(call.LocationX, call.LocationY);
            return nearbyVolunteers.Take(20).ToList();
        }
    }
}