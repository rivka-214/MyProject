using AutoMapper;
using Common.Dto;
using Reposetory.Entities;
using Repository.Entities;
using Repository.Interfacese;
using Repository.Repositories;
using Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Services
{
    public class VolunteersCallService : IService<VolunteerCallsDto>, IVolunteersCallLogic
    {
        private readonly IRepository<VolunteerCalls> repository;
        private readonly IMapper mapper;
        private readonly IVolunteerLogic volunteerLogic;
        private readonly ICallService callService;

        public VolunteersCallService(
            IRepository<VolunteerCalls> repository,
            IMapper mapper,
            IVolunteerLogic volunteerLogic,
            ICallService callService)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.volunteerLogic = volunteerLogic;
            this.callService = callService;
        }

        public async Task<VolunteerCallsDto> AddItemAsync(VolunteerCallsDto item)
        {
            var entity = mapper.Map<VolunteerCalls>(item);
            var added = await repository.AddItem(entity);
            return mapper.Map<VolunteerCallsDto>(added);
        }

        public async Task DeleteItemAsync(int id)
        {
            await repository.DeleteItem(id);
        }

        public async Task<List<VolunteerCallsDto>> GetAllAsync()
        {
            var list = await repository.GetAll();
            return mapper.Map<List<VolunteerCallsDto>>(list);
        }

        public async Task<VolunteerCallsDto> GetByIdAsync(int id)
        {
            var item = await repository.GetById(id);
            return mapper.Map<VolunteerCallsDto>(item);
        }

        public async Task UpdateItemAsync(int id, VolunteerCallsDto item)
        {
            var entity = mapper.Map<VolunteerCalls>(item);
            await repository.UpdateItem(id, entity);
        }

        public async Task AssignNearbyVolunteersToCall(int callId, double locationX, double locationY)
        {
            var nearbyVolunteers = await volunteerLogic.GetNearbyVolunteers(locationX, locationY);
            foreach (var volunteer in nearbyVolunteers)
            {
                var newItem = new VolunteerCallsDto
                {
                    CallsId = callId,
                    VolunteerId = volunteer.Id,
                    VolunteerStatus = "notified"
                };
                var entity = mapper.Map<VolunteerCalls>(newItem);
                await repository.AddItem(entity);
            }
        }

        public async Task<List<VolunteerCallsDto>> GetActiveCallsForVolunteer(int volunteerId)
        {
            var repo = repository as VolunteersCallsRepository;
            var activeCalls = await repo?.GetActiveCallsForVolunteer(volunteerId) ?? new List<VolunteerCalls>();
            return mapper.Map<List<VolunteerCallsDto>>(activeCalls);
        }

        public async Task<List<VolunteerCallsDto>> GetHistoryCallsForVolunteer(int volunteerId)
        {
            var repo = repository as VolunteersCallsRepository;
            var historyCalls = await repo?.GetHistoryCallsForVolunteer(volunteerId) ?? new List<VolunteerCalls>();
            return mapper.Map<List<VolunteerCallsDto>>(historyCalls);
        }

        public async Task RespondToCall(int callId, int volunteerId, string response)
        {
            var repo = repository as VolunteersCallsRepository;
            await repo?.UpdateVolunteerStatus(callId, volunteerId, response);

            if (response == "going")
            {
                // קריאה נשארת פתוחה
            }
            else if (response == "arrived")
            {
                await callService.UpdateStatus(callId, "in_progress");
            }
        }

        public async Task UpdateVolunteerStatus(int callId, int volunteerId, string status, string summary = null)
        {
            var repo = repository as VolunteersCallsRepository;
            await repo?.UpdateVolunteerStatus(callId, volunteerId, status);

            if (status == "arrived")
            {
                await callService.UpdateStatus(callId, "in_progress");
            }
            else if (status == "finished")
            {
                if (!string.IsNullOrEmpty(summary))
                {
                    var completeDto = new CompleteCallDto
                    {
                        Summary = summary,
                        SentToHospital = false
                    };
                    await callService.CompleteCall(callId, completeDto);
                }
                await callService.UpdateStatus(callId, "closed");
            }
        }

        public async Task<bool> ShouldSendToMoreVolunteers(int callId)
        {
            var repo = repository as VolunteersCallsRepository;
            if (repo == null)
                return false;

            var goingCount = await repo.GetGoingVolunteersCount(callId);
            return goingCount < 3;
        }

        public async Task<string> GetCallVolunteersInfo(int callId)
        {
            var repo = repository as VolunteersCallsRepository;
            if (repo == null)
                return "שגיאה פנימית";

            var goingCount = await repo.GetGoingVolunteersCount(callId);
            var hasArrived = await repo.HasArrivedVolunteer(callId);

            if (hasArrived)
                return "מתנדב הגיע למקום - בטיפול";
            else if (goingCount > 0)
                return $"{goingCount} מתנדבים יצאו לקריאה";
            else
                return "ממתין למתנדבים";
        }
    }
}
