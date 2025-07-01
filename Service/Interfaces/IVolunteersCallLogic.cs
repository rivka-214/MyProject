using Common.Dto;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IVolunteersCallLogic
    {
        Task AssignNearbyVolunteersToCall(int callId, double locationX, double locationY);
        Task RespondToCall(int callId, int volunteerId, string response, int currentVolunteerId);
        Task UpdateVolunteerStatus(int callId, int volunteerId, string status, int currentVolunteerId, string summary = null);
        Task<bool> ShouldSendToMoreVolunteers(int callId);
        Task<CallVolunteersInfoDto> GetCallVolunteersInfo(int callId); // תיקון חתימה
      
        Task<string> GetVolunteerStatus(int callId, int volunteerId);
        Task<int> GetGoingVolunteersCount(int callId);
        Task<bool> HasArrivedVolunteer(int callId);
        Task<VolunteerCallsDto> GetVolunteerCall(int callId, int volunteerId);
        Task<List<VolunteerCallsDto>> GetActiveCallsForVolunteer(int volunteerId); // הוספה
        Task<List<VolunteerCallsDto>> GetHistoryCallsForVolunteer(int volunteerId); // הוספה
        public Task CheckAndReassignVolunteers();
    }
}