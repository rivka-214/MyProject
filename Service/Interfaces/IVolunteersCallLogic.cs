using Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IVolunteersCallLogic
    {
        Task<List<VolunteerCallsDto>> GetActiveCallsForVolunteer(int volunteerId);
        Task<List<VolunteerCallsDto>> GetHistoryCallsForVolunteer(int volunteerId);
       
        Task AssignNearbyVolunteersToCall(int callId, double locationX, double locationY);
        Task RespondToCall(int callId, int volunteerId, string response, int currentVolunteerId);
        Task UpdateVolunteerStatus(int callId, int volunteerId, string status, int currentVolunteerId, string summary = null);
        Task<bool> ShouldSendToMoreVolunteers(int callId);
        Task<CallVolunteersInfoDto> GetCallVolunteersInfo(int callId); // תיקון חתימה
      
        Task<string> GetVolunteerStatus(int callId, int volunteerId);
        Task<int> GetGoingVolunteersCount(int callId);
        Task<bool> HasArrivedVolunteer(int callId);
        Task<VolunteerCallsDto> GetVolunteerCall(int callId, int volunteerId);
      
        public Task CheckAndReassignVolunteers();
        //Task<string> GetCallVolunteersInfo(int callId);
    }
}